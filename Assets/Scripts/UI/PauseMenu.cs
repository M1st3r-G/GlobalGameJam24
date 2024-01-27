using UnityEngine;
using UnityEngine.InputSystem;

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
    }
}