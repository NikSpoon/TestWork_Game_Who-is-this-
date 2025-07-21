using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorCategoryUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private CharacterColorData _colorData;
    [SerializeField] private CharacterColorProfile _platerColorData;
    [SerializeField] private CharacterColorizer _characterColorizer;

    [Header("UI Prefab")]
    [SerializeField] private GameObject _torsoColorsButtonPrefab;
    [SerializeField] private GameObject _skinColorButtonPrefab;
    [SerializeField] private GameObject _legColorButtonPrefab;
    [SerializeField] private GameObject _bootColorButtonPrefab;

    [Header("UI References")]
    [SerializeField] private Transform _colorListContainer;

    private enum ColorCategory { Torso, Skin, Legs, Boots }
    private ColorCategory _currentCategory;

    private void Start()
    {
        ShowTorsoColors();
    }

    public void ShowTorsoColors()
    {
        _currentCategory = ColorCategory.Torso;
        DisplayColors(_colorData.TorsoColors, _torsoColorsButtonPrefab);
    }

    public void ShowSkinColors()
    {
        _currentCategory = ColorCategory.Skin;
        DisplayColors(_colorData.SkinColors, _skinColorButtonPrefab);
    }

    public void ShowLegColors()
    {
        _currentCategory = ColorCategory.Legs;
        DisplayColors(_colorData.LegColors, _legColorButtonPrefab);
    }

    public void ShowBootColors()
    {
        _currentCategory = ColorCategory.Boots;
        DisplayColors(_colorData.BootColors, _bootColorButtonPrefab);
    }

    public void ClearColors()
    {
        foreach (Transform child in _colorListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void DisplayColors(List<Color> colors, GameObject pref)
    {
        ClearColors();

        foreach (var color in colors)
        {
            GameObject btnObj = Instantiate(pref, _colorListContainer);
            Image image = btnObj.GetComponent<Image>();
            Button button = btnObj.GetComponent<Button>();

            Color visibleColor = color;
            visibleColor.a = 1f;
            image.color = visibleColor;

            if (button != null)
            {
                button.onClick.AddListener(() => ApplyColor(color));
            }
        }
    }

    private void ApplyColor(Color color)
    {
        switch (_currentCategory)
        {
            case ColorCategory.Torso:
                _platerColorData.Torso = color;
                break;
            case ColorCategory.Skin:
                _platerColorData.Skin = color;
                break;
            case ColorCategory.Legs:
                _platerColorData.Legs = color;
                break;
            case ColorCategory.Boots:
                _platerColorData.Boots = color;
                break;
        }
       

        if (_characterColorizer != null)
            _characterColorizer.UpdateColorsFromProfile();
    }
}

