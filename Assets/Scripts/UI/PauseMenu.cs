using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : PopUpMenu
    {
        //ComponentReferences
        [SerializeField] private InputActionReference pauseAction;
        //Params
        //Temps
        //Publics

        private void OnEnable() {
            pauseAction.action.Enable();
            pauseAction.action.performed += OnPause; 
        }

        private void OnDisable() {
            pauseAction.action.performed -= OnPause;
            pauseAction.action.Disable();
        }
        
        private void OnPause(InputAction.CallbackContext ctx) => ToggleMenu();
        public void ContinueButton() => ToggleMenu();
        public void QuitMainMenu() => SceneManager.LoadScene(0);
        public void QuitFull() => Application.Quit();
    }
}