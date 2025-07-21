using FSM;
using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UI Screens")]
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _gameplayScreen;
    [SerializeField] private GameObject _loadScreen;

    [SerializeField] private Transform _root;

    private GameObject _currentScreen;
   
    private void Start()
    {
        StartCoroutine(WaitForPlayerDataManager());
        _currentScreen = Instantiate(_loadingScreen, _root);
    }

    private void OnStateChange(StateChangeData<AppState, AppTriger> data)
    {
        if (_currentScreen != null)
        {
            Destroy(_currentScreen);
        }
      
        switch (data.NewState)
        {
            case AppState.MainMenu:
                _currentScreen = Instantiate(_mainMenuScreen, _root);
                break;
            case AppState.Gameplay:
                _currentScreen = Instantiate(_gameplayScreen, _root);
                break;
            case AppState.Loading:
                _currentScreen = Instantiate(_loadingScreen, _root);
                break;

        }

    }
    public void ShowLoading()
    {
        if (_loadScreen != null)
            _loadScreen.SetActive(true);
    }

    public void HideLoading()
    {
        if (_loadScreen != null)
            _loadScreen.SetActive(false);
    }
    private IEnumerator WaitForPlayerDataManager()
    {
        while (GameManager.Instance == null)
            yield return null;

        var appSystem = GameManager.Instance;
        appSystem.OnStateChange += OnStateChange;

        DontDestroyOnLoad(gameObject);
    }
  
}