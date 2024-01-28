using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    //ComponentReferences
    private AudioSource source;
    //Params
    [SerializeField] private AudioClip[] clips;
    //Temps
    //Publics
    public static SoundManager Instance { get; private set; }

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
        if (Instance is not null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        source = GetComponent<AudioSource>();
    }
    
    private void OnDestroy() {
        if(Instance == this) Instance = null;
    }

    public void PlaySound(int index) {
        print("sound");
        source.PlayOneShot(clips[index]);
    }

    public void PlayMusic(int index) {
        source.clip = clips[index];
        source.Play();
    }
}