using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    //ComponentReferences
    public RuntimeAnimatorController GetAnimationController => anim;
    [SerializeField] private RuntimeAnimatorController anim;
    public Sprite DefaultSprite => dSprite;
    [SerializeField] private Sprite dSprite;
    //Params
}