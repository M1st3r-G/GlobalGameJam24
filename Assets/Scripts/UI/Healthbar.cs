using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class Healthbar : MonoBehaviour
    {
        //ComponentReferences
        //Params
        [SerializeField] private Slider[] sliders;
        //Temps
        //Public

        private void Start() {
            for (int i = 0; i < GameManager.Instance.GetComponent<PlayerInputManager>().playerCount; i++)
            {
                sliders[i].gameObject.SetActive(true);
                sliders[i].value = 0f;
            }
        }
    }
}