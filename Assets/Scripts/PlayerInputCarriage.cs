using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCarriage : MonoBehaviour
{
    //ComponentReferences
    //Params
    //Temps
    private Dictionary<InputDevice, Color> data;
    //Public
    public static PlayerInputCarriage Instance;     
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        data = new Dictionary<InputDevice, Color>();
        foreach (KeyValuePair<InputDevice, Color> player in SelectionScreenManager.Instance.GetData())
        {
            print("" + player.Key + player.Value);
            data.Add(player.Key, player.Value);
        }
    }
    
    public Dictionary<InputDevice, Color> GetData() {
        Destroy(gameObject);
        return data;
    }
}