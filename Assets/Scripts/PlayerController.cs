using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {
    //ComponentReferences
    private InputAction move;
    private Rigidbody2D rb;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject hitBoxBig;
    //Params
    [SerializeField] [Range(0f,10f)] private float speed;
    [SerializeField] [Range(0f, 10f)] private float jumpHeight;
    [SerializeField] private int maxHealth;
    //Temps
    private int currentHealth;
    //Public
    public enum Directions {
        Up,
        Down,
        Left,
        Right
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerInput>().actions.FindAction("Move");
        currentHealth = maxHealth;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(move.ReadValue<float>() * speed, rb.velocity.y);
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    private void LightAttack(Directions direction)
    {
        Vector2 pos = transform.position;
        switch (direction)
        {
            case Directions.Up:
                pos += Vector2.up;
                break;
            case Directions.Down:
                Debug.LogWarning("DownAttackWeak");
                break;
            case Directions.Left:
            case Directions.Right:
                pos += direction == Directions.Left ? Vector2.left : Vector2.right;
                break;
        }

        Destroy(Instantiate(hitBox, pos, Quaternion.identity), 0.5f);
    }

    private void HeavyAttack(Directions direction) {
        Vector2 pos = transform.position;
        switch (direction)
        {
            case Directions.Up:
                pos += Vector2.up;
                break;
            case Directions.Down:
                Debug.LogWarning("DownAttackHeavy");
                break;
            case Directions.Left:
            case Directions.Right:
                pos += direction == Directions.Left ? Vector2.left : Vector2.right;
                break;
        }

        Destroy(Instantiate(hitBoxBig, pos, Quaternion.identity), 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ChangeHealth(-1);
    }

    public void ChangeHealth(int amount) {
        currentHealth = Mathf.Clamp(amount + currentHealth, 0, maxHealth);
        print(currentHealth);
        if (currentHealth == 0) Death();
    }

    private void Death() {
        print(gameObject.name + " died");
        Destroy(gameObject);
    }

    public void OnLightAttackUp(InputAction.CallbackContext ctx) {
        if(ctx.performed) LightAttack(Directions.Up);
    }

    public void OnLightAttackLeft(InputAction.CallbackContext ctx) {
        if(ctx.performed) LightAttack(Directions.Left);
    }

    public void OnLightAttackRight(InputAction.CallbackContext ctx) {
        if(ctx.performed) LightAttack(Directions.Down);
    }

    public void OnLightAttackDown(InputAction.CallbackContext ctx) {
        if(ctx.performed) LightAttack(Directions.Down);
    }

    public void OnHeavyAttackUp(InputAction.CallbackContext ctx) {
        if(ctx.performed) HeavyAttack(Directions.Up);
    }

    public void OnHeavyAttackLeft(InputAction.CallbackContext ctx) {
        if(ctx.performed) HeavyAttack(Directions.Left);
    }

    public void OnHeavyAttackRight(InputAction.CallbackContext ctx) {
        if (ctx.performed) HeavyAttack(Directions.Right);
    }

    public void OnHeavyAttackDown(InputAction.CallbackContext ctx) {
        if (ctx.performed) HeavyAttack(Directions.Down);
    }
}