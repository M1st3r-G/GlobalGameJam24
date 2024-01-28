using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

namespace UI
{
    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UltimateVideoController : MonoBehaviour
    {
        //ComponentReferences
        [SerializeField] private InputActionAsset playerActions;
        private VideoPlayer vid;
        private CanvasGroup group;
        //Params
        //Temps
        //Public
        public static UltimateVideoController Instance;
        
        private void Awake() {
            group = GetComponent<CanvasGroup>();
            vid = GetComponent<VideoPlayer>();
            Instance = this;
        }

        public float PlayVideo(CharacterData data) {
            group.alpha = 1f;
            Time.timeScale = 0f;
            playerActions.Disable();
            vid.clip = data.ultimateVideo;
            vid.Play();
            StartCoroutine(CleanUpAfterTime((float) vid.clip.length));
            return (float) vid.clip.length;
        }

        private IEnumerator CleanUpAfterTime(float t) {
            yield return new WaitForSecondsRealtime(t);
            Time.timeScale = 1f;
            playerActions.Enable();
            group.alpha = 0f;
        }
    }
}