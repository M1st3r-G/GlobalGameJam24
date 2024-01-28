using System.Collections;
using UI;
using Unity.VisualScripting;
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
    private static readonly int UltimateBool = Animator.StringToHash("Ultimate");
    
    //ComponentReferences
    private InputAction move;
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerInput input;
    private CharacterData currentCharacter;
    private SpriteRenderer sr;
    
    //Params
    [Header("Base Stats")]
    [SerializeField] [Range(0f, 10f)] private float speed;
    [SerializeField] [Range(0f, 10f)] private float jumpHeight;
    [SerializeField] [Range(1f, 10f)] private int maxHealth;
    
    [Header("Attack Stuff")]
    [SerializeField] [Range(1f, 10f)] private int damageLight;
    [SerializeField] [Range(1f, 10f)] private int damageHeavy;
    [SerializeField] [Range(0f, 5f)] private float lightAttackLimit;
    
    [Header("Damage Indicator")]
    [SerializeField] private Color damageColor;
    [SerializeField] private float secondsDamageIndicator;
    
    [Header("Artist Suck")]
    [SerializeField] private bool spriteWrongWay;

    [Header("Ult Stuff")] 
    private UltChargesController chargeController;
    [SerializeField] private int neededUltCharge;
    [SerializeField] private int chargeFromDamageTaken;
    [SerializeField] private int chargeFromLightAttack;
    [SerializeField] private int chargeFromHeavyAttack;
    
    //Temps
    private float lastAttackTime;
    private int currentHealth;
    private bool lookingRight;
    private int ultCharge;
    private Coroutine steps;
    private bool onGround;

    //Public
    public delegate void PlayerDeathDelegate(PlayerInput player);
    public static PlayerDeathDelegate OnPlayerDeath;

    public enum Direction {
        Up,
        Down,
        Side
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerInput>().actions.FindAction("Move");
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        input = GetComponent<PlayerInput>();
        chargeController = GameObject.FindGameObjectWithTag("UltiBar").GetComponent<UltChargesController>(); 
        currentHealth = maxHealth;
    }

    public void OnUpModifier(InputAction.CallbackContext ctx) {
        if (ctx.started) anim.SetBool(UpModifierBool, true);
        else if (ctx.canceled) anim.SetBool(UpModifierBool, false);
    }

    public void OnDownModifier(InputAction.CallbackContext ctx) {
        if (ctx.started) anim.SetBool(DownModifierBool, true);
        else if (ctx.canceled) anim.SetBool(DownModifierBool, false);
    }

    private void FixedUpdate() {
        float dir = move.ReadValue<float>() * speed;
        rb.velocity = new Vector2(dir, rb.velocity.y);
        if (Mathf.Abs(rb.velocity.x) > 0) lookingRight = dir > 0;
        anim.SetBool(LookingRightBool, lookingRight);
        sr.flipX = spriteWrongWay ? lookingRight : !lookingRight;
        if (Mathf.Abs(rb.velocity.x) > 0 && onGround) {
            if (steps != null) {
                steps = StartCoroutine(StepSound());
            }
        } else {
            StopCoroutine(StepSound());  
        }
    }

    private IEnumerator StepSound() {
        SoundManager.Instance.PlaySound(SoundManager.PlayerStep);
        yield return new WaitForSeconds(3);
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << 3);
        anim.SetBool(JumpBool, hit.collider is not null);
    }

    private void TriggerJump() {
        anim.SetBool(JumpBool, false);
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        onGround = false;
    }

    public void OnAttack(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            anim.SetBool(AttackBool, true);
            lastAttackTime = Time.time;
        }

        if (!ctx.canceled) return;
        anim.SetBool(AttackBool, false);
        anim.SetBool(Time.time - lastAttackTime > lightAttackLimit ? HeavyAttackBool : LightAttackBool, true);
    }

    public void OnUlt(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return; 
        if (ultCharge < neededUltCharge) return;
        
        print( gameObject.name + " HAT SEINE ULTIMATIVE FÄHIGKEIT BENUTZT!");
        anim.SetBool(UltimateBool, true);
        ultCharge = 0;
    }

    private void TriggerUltimate() {
        anim.SetBool(UltimateBool, false);
    }
    
    private void TriggerLightAttack(Direction dir) {
        anim.SetBool(LightAttackBool, false);

        Vector2 pos = OffsetPosition(dir);

        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 1);
        foreach (var hit in hits) {
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit is null || playerHit == this) continue;
            playerHit.ChangeHealth(-damageLight);
            ChangeChargeBar(chargeFromLightAttack);
            SoundManager.Instance.PlaySound(SoundManager.PlayerPunch);
        }
    }

    private Vector2 OffsetPosition(Direction dir) {
        return (Vector2)transform.position + dir switch {
            Direction.Up => 2.5f * Vector2.up,
            Direction.Down => Vector2.down,
            _ => Vector2.up + (lookingRight ? Vector2.right : Vector2.left)
        };
    }

    private void TriggerHeavyAttack(Direction dir) {
        anim.SetBool(HeavyAttackBool, false);

        Vector2 pos = OffsetPosition(dir);

        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 1);
        foreach (var hit in hits) {
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit is null || playerHit == this) continue;
            playerHit.ChangeHealth(-damageHeavy);
            ChangeChargeBar(chargeFromHeavyAttack);
            SoundManager.Instance.PlaySound(SoundManager.PlayerPunch);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Death();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Platform")) transform.SetParent(other.transform);
        RaycastHit2D checkGround = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1<<3);
        if (checkGround.collider != null) onGround = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Platform")) transform.SetParent(null);
        RaycastHit2D checkGround = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1<<3);
        if (checkGround.collider == null) onGround = false;
    }

    private void ChangeHealth(int amount) {
        currentHealth = Mathf.Clamp(amount + currentHealth, 0, maxHealth);
        StartCoroutine(Indicator(damageColor, secondsDamageIndicator));
        ChangeChargeBar(chargeFromDamageTaken);
        if (currentHealth == 0) Death();
    }

    private void ChangeChargeBar(int amount) {
        ultCharge += amount;
        if (ultCharge > neededUltCharge) ultCharge = neededUltCharge;
        chargeController.ChangeValue(input.playerIndex, (float) ultCharge / neededUltCharge);
    }

    private IEnumerator Indicator(Color color, float seconds) {
        sr.color = color;
        yield return new WaitForSeconds(seconds);
        sr.color = Color.white;
    }

    private void Death() {
        print(gameObject.name + " died. Your mom is disappointed.");
        OnPlayerDeath?.Invoke(GetComponent<PlayerInput>());
        SoundManager.Instance.PlaySound(SoundManager.PlayerDeath);
        Destroy(gameObject);
    }

    public void SetCharacter(CharacterData character) {
        currentCharacter = character;
        anim.runtimeAnimatorController = currentCharacter.GetAnimationController;
        spriteWrongWay = currentCharacter.DefaultIsLookingLeft; 
    }
}