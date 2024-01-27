using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDisplay : MonoBehaviour
{
    //ComponentReferences
    private SpriteRenderer sr;
    //Params
    //Temps
    private int currentCharacterIndex;
    //Public

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        sr.color = SelectionScreenManager.Instance.GetAvailablePlayer(-1, out currentCharacterIndex);
    }

    public void ChangeToNext(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        sr.color = SelectionScreenManager.Instance.GetAvailablePlayer(currentCharacterIndex, out currentCharacterIndex);
    }

    public void Leave(InputAction.CallbackContext ctx) {
        SelectionScreenManager.Instance.OnPlayerLeave(currentCharacterIndex);
        Destroy(gameObject);
    }
}