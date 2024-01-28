using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //ComponentReferences
    private AudioSource source;
    //Params
    [SerializeField] private AudioClip[] clips;
    //Temps
    private static SoundManager instance;
    //Publics
    public static SoundManager Instance => instance;
    
    public const int PlayerPunch = 0;
    public const int PlayerStep = 1;
    public const int PlayerDeath = 2;
    public const int ButtonClick = 3;

    public const int MapCat = 4;
    public const int MapLSD = 5;
    public const int PolishMap = 6;
    public const int CYFMH = 7;

    public const int WinCat = 8;
    public const int WinGigaChad = 9;
    public const int WinPepe = 10;
    public const int WinPete = 11;
     
    private void Awake() {
        if (instance is not null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        source = GetComponent<AudioSource>();
    }
    
    private void OnDestroy() {
        if(Instance == this) instance = null;
    }

    public void PlaySound(int index) {
        source.PlayOneShot(clips[index]);
    }

    public void PlayMusic(int index) {
        source.clip = clips[index];
        source.Play();
    }
}