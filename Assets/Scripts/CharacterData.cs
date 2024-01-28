using UnityEngine;

[CreateAssetMenu(menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    //ComponentReferences
    public RuntimeAnimatorController GetAnimationController => anim;
    [SerializeField] private RuntimeAnimatorController anim;
    public Sprite DefaultSprite => dSprite;
    [SerializeField] private Sprite dSprite;
    public bool DefaultIsLookingLeft => isLookingLeft;
    [SerializeField] private bool isLookingLeft;
    public Sprite WinSprite => wSprite;
    [SerializeField] private Sprite wSprite;
    //Params
}