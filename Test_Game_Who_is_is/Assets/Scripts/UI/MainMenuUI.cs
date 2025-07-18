using Assets.Scripts.GameAppControl;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _skinPanel;
    [SerializeField] private GameObject _optionsPanel;

    public void OnPlay()
    {
        GameManager.Instance.Trigger(AppTriger.ToGameplay);
    }
    public void OnSkin()
    {
        if (_optionsPanel.activeSelf)
            _optionsPanel.SetActive(false);

        _skinPanel.SetActive(true);
    }
    public void OnOptions()
    {
        if (_skinPanel.activeSelf)
            _skinPanel.SetActive(false);

        _optionsPanel.SetActive(true);
    }
    public void OnExit()
    {
        Application.Quit();
    }
}
