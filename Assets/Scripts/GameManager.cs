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
    
    public void OnPlayerLeave(PlayerInput player) => Debug.LogWarning("Player Disconnected");
    public void OnPlayerJoined(PlayerInput player) {
        OnPlayerJoin?.Invoke(player.GetComponent<PlayerController>());
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}