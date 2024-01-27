using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {
    private static readonly int UpModifierBool = Animator.StringToHash("Up");
    private static readonly int DownModifierBool = Animator.StringToHash("Down");
    private static readonly int LookingRightBool = Animator.StringToHash("LookingRight");
    private static readonly int JumpBool = Animator.StringToHash("Jump");
    private static readonly int AttackBool = Animator.StringToHash("Attack");
    private static readonly int HeavyAttackBool = Animator.StringToHash("Heavy");
    private static readonly int LightAttackBool = Animator.StringToHash("Light");
    
    //ComponentReferences
    private InputAction move;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private GameObject DebugCollider;
    //Params
    [SerializeField] [Range(0f,10f)] private float speed;
    [SerializeField] [Range(0f, 10f)] private float jumpHeight;
    [SerializeField] [Range(1f, 10f)] private int maxHealth;
    [SerializeField] [Range(1f, 10f)] private int damageLight;
    [SerializeField] [Range(1f, 10f)] private int damageHeavy;
    [SerializeField] [Range(0f, 5f)] private float lightAttackLimit;
    //Temps
    private float lastAttackTime;
    private int currentHealth;
    private bool lookingRight;
    private MovingPlatform platform;
    //Public
    public delegate void PlayerDeathDelegate(PlayerInput player);
    public static PlayerDeathDelegate OnPlayerDeath;

    public enum Direction
    {
        Up, Down, Side
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerInput>().actions.FindAction("Move");
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    
    public void OnUpModifier(InputAction.CallbackContext ctx) {
        if(ctx.started) anim.SetBool(UpModifierBool, true);
        else if (ctx.canceled) anim.SetBool(UpModifierBool, false);
    }

    public void OnDownModifier(InputAction.CallbackContext ctx) {
        if(ctx.started) anim.SetBool(DownModifierBool, true);
        else if (ctx.canceled) anim.SetBool(DownModifierBool, false);
    }

    private void FixedUpdate() {
        float dir = move.ReadValue<float>() * speed;
        lookingRight = dir > 0;
        anim.SetBool(LookingRightBool, lookingRight);
        rb.velocity = new Vector2(dir, rb.velocity.y);
        /*+ (platform is null
            ? Vector2.zero
            : new Vector2(platform.Movement.x, Mathf.Min(platform.Movement.y, 0)));*/
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1<<3);
        anim.SetBool(JumpBool, hit.collider is not null);
    }

    private void TriggerJump() {
        anim.SetBool(JumpBool, false);
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    public void OnAttack(InputAction.CallbackContext ctx) {
        if (ctx.started)
        {
            anim.SetBool(AttackBool, true);
            lastAttackTime = Time.time;
        }

        if (!ctx.canceled) return;
        anim.SetBool(AttackBool, false);
        anim.SetBool(Time.time - lastAttackTime > lightAttackLimit ? HeavyAttackBool : LightAttackBool, true);
    }

    private void TriggerLightAttack(Direction dir) {
        anim.SetBool(LightAttackBool, false);

        Vector2 pos = OffsetPosition(dir);

        Destroy(Instantiate(DebugCollider, pos, Quaternion.identity), 0.5f);
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 1);
        foreach (var hit in hits) {
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit is null || playerHit == this) continue;
            playerHit.ChangeHealth(-damageLight);
        }
    }

    private Vector2 OffsetPosition(Direction dir) {
        return (Vector2) transform.position + dir switch
        {
            Direction.Up => 2.5f*Vector2.up,
            Direction.Down => Vector2.down,
            _ => Vector2.up + (lookingRight ? Vector2.right : Vector2.left)
        };
    }
    
    private void TriggerHeavyAttack(Direction dir) {
        anim.SetBool(HeavyAttackBool, false);
        
        Vector2 pos = OffsetPosition(dir);
        
        Destroy(Instantiate(DebugCollider, pos, Quaternion.identity), 0.5f);
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 1);
        foreach (var hit in hits) {
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit is null || playerHit == this) continue;
            playerHit.ChangeHealth(-damageHeavy);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        Death();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Platform")) transform.SetParent(other.transform);
        
        if (!other.gameObject.CompareTag("Platform")) return;
        RaycastHit2D leftHit = Physics2D.Raycast(new Vector2(transform.position.x + 0.49f, transform.position.y - 0.3f), Vector2.down, 0.6f, 1<<3);
        RaycastHit2D rightHit = Physics2D.Raycast(new Vector2(transform.position.x - 0.49f, transform.position.y - 0.3f), Vector2.down, 0.6f, 1<<3);
        if (leftHit.collider is not null) {
            if(!leftHit.transform.CompareTag("Platform")) return;
        }
        if (rightHit.collider is not null) {
            if(!rightHit.transform.CompareTag("Platform")) return;
        }
        Debug.LogWarning(other.gameObject.GetComponent<MovingPlatform>());
        platform = other.gameObject.GetComponent<MovingPlatform>();
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Platform")) transform.SetParent(null);
        if (other.gameObject.CompareTag("Platform")) platform = null;
    }

    private void ChangeHealth(int amount) {
        currentHealth = Mathf.Clamp(amount + currentHealth, 0, maxHealth);
        print(currentHealth);
        if (currentHealth == 0) Death();
    }

    private void Death() {
        print(gameObject.name + " died. Your mom is disappointed.");
        OnPlayerDeath?.Invoke(GetComponent<PlayerInput>());
        Destroy(gameObject);
    }
}