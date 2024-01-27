using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    
    //ModifierReferences
    [SerializeField] private InputActionReference upModifier;
    [SerializeField] private InputActionReference rightModifier;
    [SerializeField] private InputActionReference downModifier;
    [SerializeField] private InputActionReference leftModifier;
    
    //ComponentReferences
    private InputAction move;
    private Rigidbody2D rb;
    //Params
    [SerializeField] [Range(0f,10f)] private float speed;
    [SerializeField] [Range(0f, 10f)] private float jumpHeight;
    [SerializeField] [Range(1f, 10f)] private int maxHealth;
    [SerializeField] [Range(1f, 10f)] private int damageLight;
    [SerializeField] [Range(1f, 10f)] private int damageHeavy;
    //Temps
    private int currentHealth;
    private bool lookingRight;
    //Public
    public delegate void PlayerDeathDelegate(PlayerInput player);
    public static PlayerDeathDelegate OnPlayerDeath;

    private void OnEnable() {
        upModifier.action.Enable();
        rightModifier.action.Enable();
        downModifier.action.Enable();
        leftModifier.action.Enable();
    }


    private void OnDisable() {
        upModifier.action.Disable();
        rightModifier.action.Disable();
        downModifier.action.Disable();
        leftModifier.action.Disable();
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerInput>().actions.FindAction("Move");
        currentHealth = maxHealth;
    }

    private void FixedUpdate() {
        float dir = move.ReadValue<float>() * speed;
        lookingRight = dir > 0;
        rb.velocity = new Vector2(dir, rb.velocity.y);
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1<<3);
        if (hit.collider is not null) 
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    public void OnLightAttack(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        print("LightAttack");
        
        Vector2 pos = transform.position;

        if (upModifier.action.IsPressed()) pos += Vector2.up;
        else if (downModifier.action.IsPressed()) pos += Vector2.down;
        else pos += lookingRight ? Vector2.right : Vector2.left;

        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 1);
        foreach (var hit in hits) {
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit is null || playerHit == this) continue;
            playerHit.ChangeHealth(-damageLight);
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        print("HeavyAttack");

        Vector2 pos = transform.position;

        if (upModifier.action.IsPressed()) pos += Vector2.up;
        else if (downModifier.action.IsPressed()) pos += Vector2.down;
        else pos += lookingRight ? Vector2.right : Vector2.left;

        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 1);
        foreach (var hit in hits) {
            PlayerController playerHit = hit.GetComponent<PlayerController>();
            if (playerHit is null || playerHit == this) continue;
            playerHit.ChangeHealth(-damageHeavy);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ChangeHealth(-1);
    }

    private void ChangeHealth(int amount) {
        currentHealth = Mathf.Clamp(amount + currentHealth, 0, maxHealth);
        print(currentHealth);
        if (currentHealth == 0) Death();
    }

    private void Death() {
        print(gameObject.name + " died");
        OnPlayerDeath?.Invoke(GetComponent<PlayerInput>());
        Destroy(gameObject);
    }
}