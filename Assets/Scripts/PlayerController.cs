using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    //ComponentReferences
    private InputAction move;
    private InputAction jump;
    private Rigidbody2D rb;
    //Params
    [SerializeField][Range(0,10)]
    private float speed;
    [SerializeField] [Range(0, 10)]
    private float jumpHeight;
    //Temps
    //Publics
    public enum Directions {
        Up,
        Down,
        Left,
        Right
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<PlayerInput>().actions.FindAction("Move");
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(move.ReadValue<float>() * speed, rb.velocity.y);
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
    }

    private void LightAttack(Directions direction) {
        print("Light Attack");
    }

    private void HeavyAttack(Directions direction) {
        print("Heavy Attack");
    }
    
    

    public void OnLightAttackUp(InputAction.CallbackContext ctx) => LightAttack(Directions.Up);
    public void OnLightAttackLeft(InputAction.CallbackContext ctx) => LightAttack(Directions.Left);
    public void OnLightAttackRight(InputAction.CallbackContext ctx) => LightAttack(Directions.Down);
    public void OnLightAttackDown(InputAction.CallbackContext ctx) => LightAttack(Directions.Down);
    public void OnHeavyAttackUp(InputAction.CallbackContext ctx) => HeavyAttack(Directions.Up);
    public void OnHeavyAttackLeft(InputAction.CallbackContext ctx) => HeavyAttack(Directions.Left);
    public void OnHeavyAttackRight(InputAction.CallbackContext ctx) => HeavyAttack(Directions.Right);
    public void OnHeavyAttackDown(InputAction.CallbackContext ctx) => HeavyAttack(Directions.Down);
}