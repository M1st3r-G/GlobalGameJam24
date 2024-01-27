using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class PopUpMenu: MonoBehaviour
    {
        //ComponentReferences
        private CanvasGroup group;
        [SerializeField] private EventSystem controllerUI;
        [SerializeField] protected InputActionAsset playerActions;
        [SerializeField] private GameObject firstSelectionInMenu;
        [SerializeField] private TextMeshProUGUI countdownText;
        //Params
        [SerializeField] private float fadeInTime;
        [SerializeField] private float countdownTime;
        [SerializeField] private float timeTime;
        //Temps
        //Public
        private bool isActive;
        private Coroutine currentTransfer;


        protected void Awake()
        {
            group = GetComponent<CanvasGroup>();
            isActive = false;
        }

        protected void ToggleMenu()
        {
            if (currentTransfer is not null) StopCoroutine(currentTransfer);
            currentTransfer = StartCoroutine(isActive ? FadeOutRoutine() : FadeInRoutine());
        }

        private IEnumerator FadeInRoutine() {
            group.interactable = group.blocksRaycasts = isActive = true;
            controllerUI.SetSelectedGameObject(firstSelectionInMenu);
            Time.timeScale = 0f;
            playerActions.actionMaps[0].Disable();

        
            float counter = 0f;
            while (group.alpha < 1f)
            {
                group.alpha = Mathf.Lerp(0f, 1f, counter / fadeInTime);
                counter += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        
        private IEnumerator FadeOutRoutine()
        {
            group.interactable = group.blocksRaycasts = isActive = false;
        
            float counter = 0f;
            while (group.alpha > 0)
            {
                group.alpha = Mathf.Lerp(1f, 0f, counter / fadeInTime);
                counter += Time.unscaledDeltaTime;
                yield return null;
            }
            
            float countdown = countdownTime;
            while (countdown > 0)
            {
                countdownText.text = ((int)countdown).ToString();
                countdown -= Time.unscaledDeltaTime;
                yield return null;
            }
            
            playerActions.actionMaps[0].Enable();
            
            counter = 0f;
            while (Time.timeScale < 1f)
            {
                Time.timeScale = Mathf.Lerp(0f, 1f, counter / timeTime);
                counter += Time.unscaledDeltaTime;
                yield return null;
            }
            
            countdownText.text = "";
        }

        public void JumpTo(bool active)
        {
            if (currentTransfer is not null) StopCoroutine(currentTransfer);
        
            isActive = active;
            group.alpha = active ? 1f : 0f;
            group.interactable = active;
            group.blocksRaycasts = active;
            if (active) playerActions.actionMaps[0].Disable();
            else playerActions.actionMaps[0].Enable();

            Time.timeScale = active ? 0f : 1f;
        }
        
        protected void FadeIn() => StartCoroutine(FadeInRoutine());
    }
}
