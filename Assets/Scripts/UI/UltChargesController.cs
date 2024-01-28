using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UltChargesController : MonoBehaviour
    {
        //ComponentReferences
        //Params
        [FormerlySerializedAs("sliders")] [SerializeField] private Image[] bars;
        //Temps
        //Public

        private void Start() {
            for (int i = 0; i < GameManager.Instance.GetComponent<PlayerInputManager>().playerCount; i++)
            {
                bars[i].transform.parent.gameObject.SetActive(true);
                bars[i].fillAmount = 0f;
            }
        }

        public void ChangeValue(int index, float newValue) {
            bars[index].fillAmount = newValue;
        }
    }
}