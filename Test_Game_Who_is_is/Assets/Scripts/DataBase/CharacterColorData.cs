using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorOptions", menuName = "Character/Color Data")]
public class CharacterColorData : ScriptableObject
{
    public List<Color> TorsoColors;
    public List<Color> SkinColors;
    public List<Color> LegColors;
    public List<Color> BootColors;
}
