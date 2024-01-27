using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionScreenManager : MonoBehaviour
{
    //ComponentReferences
    //Params
    [SerializeField] private Color[] possiblePlayers;
    private bool[] availablePlayers;
    //Temps
    //Public
    public static SelectionScreenManager Instance {get; set; }
     
    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        availablePlayers = new bool[possiblePlayers.Length];
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
        player.transform.position = new Vector2(-6 + 3 * player.playerIndex, 0); 
    }
    
    public Color GetAvailablePlayer(int currentIndex, out int playerIndex) {
        playerIndex = currentIndex + 1;
        if (playerIndex == possiblePlayers.Length) playerIndex = 0;
        
        if (currentIndex != -1) availablePlayers[currentIndex] = true;
        
        for (var i = 0; i < possiblePlayers.Length; i++)
        {
            if (availablePlayers[playerIndex])
            {
                availablePlayers[playerIndex] = false;
                return possiblePlayers[playerIndex];
            }
            playerIndex++;
            if (playerIndex == possiblePlayers.Length) playerIndex = 0;
        }

        throw new Exception("Should have found a player");
    }
}