using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterSelection
{
    public class PlayerInputCarriage : MonoBehaviour
    {
        //ComponentReferences
        //Params
        //Temps
        private Dictionary<InputDevice, CharacterData> data;
        //Public
        public static PlayerInputCarriage Instance;     
        private void Awake() {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        
            data = new Dictionary<InputDevice, CharacterData>();
            foreach (KeyValuePair<InputDevice, CharacterData> player in SelectionScreenManager.Instance.GetData())
            {
                print("" + player.Key + player.Value);
                data.Add(player.Key, player.Value);
            }
        }
    
        public Dictionary<InputDevice, CharacterData> GetData() {
            Destroy(gameObject);
            return data;
        }
    }
}