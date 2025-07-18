using Assets.Scripts.GameAppControl;
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
    [SerializeField] private BaseGameState _finish;


    private Dictionary<AppState, BaseGameState> _states;
    private Dictionary<AppState, string> _sceneMap;

    private StateMashine<AppState, AppTriger> _fsm;
    private IStateLogic _currentLogic;

    public AppState CurrentState => _fsm.CurrentState;
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

        InitializeFSM();
        InitializeSceneMap();
        InitializeStates();

    }

    private void InitializeFSM()
    {
        _fsm = new StateMashine<AppState, AppTriger>(AppState.Loading);
        _fsm.OnStateChange += HandleStateChange;

        _fsm.AddTransition(AppState.Loading, AppTriger.ToMainMenu, AppState.MainMenu);
        _fsm.AddTransition(AppState.MainMenu, AppTriger.ToGameplay, AppState.Gameplay);
        _fsm.AddTransition(AppState.Gameplay, AppTriger.GameplayToGameplay, AppState.Gameplay);
        _fsm.AddTransition(AppState.Gameplay, AppTriger.ToFinish, AppState.Finish);
        _fsm.AddTransition(AppState.Finish, AppTriger.ToMainMenu, AppState.MainMenu);
    }

    private void InitializeSceneMap()
    {
        _sceneMap = new Dictionary<AppState, string>()
    {
        { AppState.Loading, "LoadingScene" },
        { AppState.MainMenu, "MenuScene" },
        { AppState.Gameplay, "GameplayScene" },
        { AppState.Finish, "FinishScene" }
    };
    }

    private void InitializeStates()
    {
        _states = new Dictionary<AppState, BaseGameState>()
    {
        { AppState.Loading, _loading },
        { AppState.MainMenu, _mainMenu },
        { AppState.Gameplay, _gameplay },
        { AppState.Finish, _finish }
    };

        foreach (var state in _states.Values)
        {
            if (state != null)
                state.Init(this);
            else
                Debug.LogWarning("One of the BaseGameState references is null.");
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


        yield return new WaitForFixedUpdate();

        if (_sceneMap.TryGetValue(newState, out var sceneName))
        {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName);
            while (!loadOp.isDone)
            {
                yield return null;
            }
        }
        yield return new WaitForFixedUpdate();
        ;
        _states[newState]?.Enter();
        _currentLogic = _states[newState];

        while (_currentLogic != null && !_currentLogic.IsReady)
            yield return new WaitForFixedUpdate();

        if (oldState.HasValue)
        {
            _states[oldState.Value]?.Exit();
        }
        _ui.HideLoading();
        OnStateChange?.Invoke(data);
        _isSwitching = false;
    }

    private void Update()
    {
        _currentLogic?.AppUpdate();
    }

    public void Trigger(AppTriger trigger)
    {
        _fsm.SetTrigger(trigger);
    }
}
