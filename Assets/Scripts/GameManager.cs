using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //ComponentReferences
    //Params
    //Temps
    //Publics
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
    }

    private void OnEnable() {
        PlayerController.OnPlayerDeath += OnPlayerLeaved;
    }

    private void OnDisable() {
        PlayerController.OnPlayerDeath -= OnPlayerLeaved;
    }

    private void OnPlayerLeaved(PlayerInput player) {
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