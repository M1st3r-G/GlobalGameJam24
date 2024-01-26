using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    //ComponentReferences
    //Params
    //Temps
    //Publics
    public static GameManager Instance {get; set; }
     
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
    public void OnPlayerJoined(PlayerInput player) => Debug.LogWarning("Player Connected");
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}