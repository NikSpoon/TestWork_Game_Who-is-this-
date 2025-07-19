using UnityEngine;
using System.Collections.Generic;

public class CharacterColorizer : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private SkinnedMeshRenderer _skinnedMesh;
    [Header("Color Source")]
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private CharacterColorData _colorOptions; 
    [SerializeField] private CharacterColorProfile _playerProfile; 

    private void Start()
    {
        if (isPlayer)
            ApplyColors(_playerProfile.Torso, _playerProfile.Skin, _playerProfile.Legs, _playerProfile.Boots);
        else
            ApplyColors(
                GetRandom(_colorOptions.TorsoColors),
                GetRandom(_colorOptions.SkinColors),
                GetRandom(_colorOptions.LegColors),
                GetRandom(_colorOptions.BootColors)
            );
    }

    private void ApplyColors(Color torso, Color skin, Color leg, Color boot)
    {
        if (_skinnedMesh == null || _baseMaterial == null) return;

        int matsCount = _skinnedMesh.sharedMaterials.Length;
        Material[] newMats = new Material[matsCount];

        for (int i = 0; i < matsCount; i++)
        {
            newMats[i] = new Material(_baseMaterial);
        }

        if (matsCount >= 1)
            newMats[0].SetColor("_BaseColor", torso);

        if (matsCount >= 2)
            newMats[1].SetColor("_BaseColor", leg);

        if (matsCount >= 3)
            newMats[2].SetColor("_BaseColor", skin);

        if (matsCount >= 4)
            newMats[3].SetColor("_BaseColor", boot);

        _skinnedMesh.materials = newMats;
    }
    private Color GetRandom(List<Color> colors)
    {
        if (colors == null || colors.Count == 0)
            return Color.white;
       
        System.Random random = new System.Random();
        return colors[random.Next(colors.Count)];
       
    }
}