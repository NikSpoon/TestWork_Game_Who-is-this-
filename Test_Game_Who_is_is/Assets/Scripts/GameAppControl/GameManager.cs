
using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IAppSystem
{

    public static GameManager Instance;

    [SerializeField] private UIController _ui;

    [SerializeField] private BaseGameState _loading;
    [SerializeField] private BaseGameState _mainMenu;
    [SerializeField] private BaseGameState _gameplay;
  

    private IAppSystem _appSystem; 
    private Dictionary<AppState, BaseGameState> _states;
    private Dictionary<AppState, string> _sceneMap;

    private IStateLogic _currentLogic;

    public AppState CurrentState => _appSystem.CurrentState;
    public event Action<StateChangeData<AppState, AppTriger>> OnStateChange;

    private bool _isSwitching = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _appSystem = new AppSystem();

        _appSystem.OnStateChange += HandleStateChange;

        InitializeSceneMap();
        InitializeStates();
    }

    private void InitializeSceneMap()
    {
        _sceneMap = new Dictionary<AppState, string>()
    {
        { AppState.Loading, "LoadingScene" },
        { AppState.MainMenu, "MenuScene" },
        { AppState.Gameplay, "GameplayScene" },
    };
    }

    private void InitializeStates()
    {
        _states = new Dictionary<AppState, BaseGameState>()
    {
        { AppState.Loading, _loading },
        { AppState.MainMenu, _mainMenu },
        { AppState.Gameplay, _gameplay },
    };

        foreach (var state in _states.Values)
        {
            if (state != null)
            {
                state.gameObject.SetActive(true); 
                state.Init(this);
                state.gameObject.SetActive(false); 
            }
        }
    }

    private void HandleStateChange(StateChangeData<AppState, AppTriger> data)
    {
        StartCoroutine(SwitchToStateRoutine(data.NewState, data.OldState, data));
    }

    private IEnumerator SwitchToStateRoutine(AppState newState, AppState? oldState, StateChangeData<AppState, AppTriger> data)
    {
        if (_isSwitching)
            yield break;

        _isSwitching = true;

        
        _ui.ShowLoading();
        yield return new WaitForEndOfFrame();

        if (oldState.HasValue && _states.TryGetValue(oldState.Value, out var oldStateObj))
        {
            oldStateObj.Exit();
            oldStateObj.gameObject.SetActive(false);
        }

        yield return new WaitForFixedUpdate(); 

        if (_sceneMap.TryGetValue(newState, out var sceneName))
        {
            var loadOp = SceneManager.LoadSceneAsync(sceneName);
            while (!loadOp.isDone)
                yield return null;
        }

        yield return new WaitForEndOfFrame();

        if (_states.TryGetValue(newState, out var newStateObj))
        {
            newStateObj.gameObject.SetActive(true);
            newStateObj.Enter();
            _currentLogic = newStateObj;
        }

       
        while (_currentLogic != null && !_currentLogic.IsReady)
            yield return new WaitForEndOfFrame();

       
        _ui.HideLoading();

        OnStateChange?.Invoke(data);
        _isSwitching = false;
    }

    private void Update()
    {
        _currentLogic?.GameUpdate();
    }

    public void Trigger(AppTriger trigger)
    {
        _appSystem.Trigger(trigger);
    }
}
