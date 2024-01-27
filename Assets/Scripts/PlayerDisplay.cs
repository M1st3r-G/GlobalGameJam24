using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerDisplay : MonoBehaviour
{
    //ComponentReferences
    private SpriteRenderer sr;

    //Params
    [SerializeField] private float holdTime;
    //Temps
    private float startHold;
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
        SelectionScreenManager.Instance.PlayerLeave(currentCharacterIndex);
        Destroy(gameObject);
    }


    public void LoadNextScene(InputAction.CallbackContext ctx) {
        if (ctx.started) startHold = Time.time;
        if (!ctx.canceled || !(Time.time - startHold > holdTime)) return;
        
        GameObject tmp =  Instantiate(new GameObject());
        tmp.AddComponent<PlayerInputCarriage>();
        SceneManager.LoadScene(1);
    }
}