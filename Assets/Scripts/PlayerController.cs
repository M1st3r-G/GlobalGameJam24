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
}