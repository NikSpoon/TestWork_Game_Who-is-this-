using UnityEngine;

[CreateAssetMenu(fileName = "PlayerColorProfile", menuName = "Character/Player Profile")]
public class CharacterColorProfile : ScriptableObject
{
    public Color Torso;
    public Color Skin;
    public Color Legs;
    public Color Boots;
}