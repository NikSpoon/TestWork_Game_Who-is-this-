using FSM;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class BaseAI<TState, TTrigger> : MonoBehaviour where TState : Enum
{
    [SerializeField] protected BaseStateAI[] _states;

    protected StateMashine<TState, TTrigger> stateMachine;
    protected Dictionary<TState, BaseStateAI> stateMap = new();
    protected abstract TState GetInitialState();

    private BaseStateAI _currentStateBehaviour;

    protected virtual void Awake()
    {
        stateMachine = new StateMashine<TState, TTrigger>(GetInitialState());
        stateMachine.OnStateChange += HandleStateChange;

        foreach (var state in _states)
        {
            var stateEnum = GetStateTypeFromInstance(state);
            stateMap[stateEnum] = state;
            state.enabled = false; 
        }

        var initial = stateMap[GetInitialState()];
        _currentStateBehaviour = initial;
        _currentStateBehaviour.enabled = true; 
        _currentStateBehaviour.OnEnter();
    }
    protected void SetTrigger(TTrigger trigger)
    {
        stateMachine.SetTrigger(trigger);
    }

    protected abstract TState GetStateTypeFromInstance(BaseStateAI state);

    private void HandleStateChange(StateChangeData<TState, TTrigger> data)
    {
        if (_currentStateBehaviour != null)
        {
            _currentStateBehaviour.OnExit();
            _currentStateBehaviour.enabled = false;
        }

        if (stateMap.TryGetValue(data.NewState, out var newState))
        {
            _currentStateBehaviour = newState;
            _currentStateBehaviour.enabled = true;
            _currentStateBehaviour.OnEnter();
        }
        else
        {
            Debug.LogError($"State {data.NewState} not registered in AI");
        }
    }

}