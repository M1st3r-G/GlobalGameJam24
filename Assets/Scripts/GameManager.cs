using System.Collections.Generic;
using CharacterSelection;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //ComponentReferences
    private PlayerInputManager inputManager;
    //Params
    //Temps
    //Public
    public static GameManager Instance {get; set; }

    public delegate void PlayerJoinDelegate(PlayerController player);
    public static PlayerJoinDelegate OnPlayerJoin;
    public delegate void PlayerLeaveDelegate(PlayerController player);
    public static PlayerLeaveDelegate OnPlayerLeave;
    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        inputManager = GetComponent<PlayerInputManager>();
    }

    private void Start() {
        Dictionary<InputDevice, CharacterData> data = PlayerInputCarriage.Instance.GetData();
        int i = 0;
        foreach (KeyValuePair<InputDevice, CharacterData> pair in data)
        {
            inputManager.JoinPlayer(i, i, null, pair.Key).GetComponent<PlayerController>().SetCharacter(pair.Value);
            i++;
        }
    }

    private void OnEnable() {
        PlayerController.OnPlayerDeath += OnPlayerLeaved;
    }

    private void OnDisable() {
        PlayerController.OnPlayerDeath -= OnPlayerLeaved;
    }

    private static void OnPlayerLeaved(PlayerInput player) {
        OnPlayerLeave?.Invoke(player.GetComponent<PlayerController>());
    }

    public void OnPlayerJoined(PlayerInput player) {
        OnPlayerJoin?.Invoke(player.GetComponent<PlayerController>());
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}