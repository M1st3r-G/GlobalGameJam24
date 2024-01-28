using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CharacterSelection
{
    public class PlayerDisplay : MonoBehaviour
    {
        //ComponentReferences
        private SpriteRenderer sr;
        private CharacterData character;
        //Params
        [SerializeField] private float holdTime;
        //Temps
        private float startHold;
        private int currentCharacterIndex;
        //Public

        private void Awake() {
            sr = GetComponent<SpriteRenderer>();
            character = SelectionScreenManager.Instance.GetAvailablePlayer(-1, out currentCharacterIndex);
            sr.sprite = character.DefaultSprite;
        }
        
        
        public void ChangeToNext(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            character = SelectionScreenManager.Instance.GetAvailablePlayer(currentCharacterIndex, out currentCharacterIndex);
            sr.sprite = character.DefaultSprite;
        }

        public void Leave(InputAction.CallbackContext ctx) {
            SelectionScreenManager.Instance.PlayerLeave(currentCharacterIndex);
            Destroy(gameObject);
        }

        public CharacterData GetCharacter() => character;
        
        public void LoadNextScene(InputAction.CallbackContext ctx) {
            if (ctx.started) startHold = Time.time;
            if (!ctx.canceled || !(Time.time - startHold > holdTime)) return;
            if (PlayerInputManager.instance.playerCount == 1)
            {
                Debug.LogError("Needs more than one Player to start");
                return;
            }
            GameObject tmp =  Instantiate(new GameObject());
            tmp.AddComponent<PlayerInputCarriage>();
            SceneManager.LoadScene(1);
        }
    }
}