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
    
    public const int PlayerDamage = 0;
     
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
}