using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CharacterSelection
{
    public class SelectionScreenManager : MonoBehaviour
    {
        //ComponentReferences
        //Params
        [SerializeField] private CharacterData[] possibleCharacters;
        private bool[] availablePlayers;
        //Temps
        private List<PlayerInput> playerData;
        //Public
        public static SelectionScreenManager Instance {get; private set; }
     
        private void Awake()
        {
            Instance = this;
            playerData = new List<PlayerInput>();
            availablePlayers = new bool[possibleCharacters.Length];
            for (int i = 0; i < availablePlayers.Length; i++)
            {
                availablePlayers[i] = true;
            }
        }
    
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void OnPlayerJoined(PlayerInput player) {
            player.transform.position = new Vector2(-6 + 4 * player.playerIndex, 0);
            playerData.Add(player);
        }

        public void OnPlayerLeave(PlayerInput player) {
            playerData.Remove(player);
        }
    
        public void PlayerLeave(int playerIndex) {
            availablePlayers[playerIndex] = true;
        }
    
        public CharacterData GetAvailablePlayer(int currentIndex, out int playerIndex) {
            playerIndex = currentIndex + 1;
            if (playerIndex == possibleCharacters.Length) playerIndex = 0;
        
            if (currentIndex != -1) availablePlayers[currentIndex] = true;
        
            for (var i = 0; i < possibleCharacters.Length; i++)
            {
                if (availablePlayers[playerIndex])
                {
                    availablePlayers[playerIndex] = false;
                    return possibleCharacters[playerIndex];
                }
                playerIndex++;
                if (playerIndex == possibleCharacters.Length) playerIndex = 0;
            }

            throw new Exception("Should have found a player");
        }
    
        public Dictionary<InputDevice, CharacterData> GetData() {
            Dictionary<InputDevice, CharacterData> tmp = new Dictionary<InputDevice, CharacterData>();
            foreach (PlayerInput input in playerData)
            {
                tmp.Add(input.devices[0], input.GetComponent<PlayerDisplay>().GetCharacter());
            }
            return tmp;
        }
    }
}