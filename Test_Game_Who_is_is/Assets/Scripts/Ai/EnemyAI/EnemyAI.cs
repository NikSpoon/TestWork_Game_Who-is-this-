using Assets.Scripts.Ai.States;
using System;
using System.Collections;
using UnityEngine;
using static EnemyAI;

public class EnemyAI : BaseAI<AIState, AITrigger>
{
    private GameplayState _gameplayState;
    private AITrigger? _lastTrigger = null;
    private bool OnPatrol = false;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Init());

        stateMachine.AddTransition(AIState.Idle, AITrigger.Start, AIState.ToSigin);
        stateMachine.AddTransition(AIState.ToSigin, AITrigger.ToPatrol, AIState.Patrol);
        stateMachine.AddTransition(AIState.Patrol, AITrigger.Idle, AIState.Idle);
    }

    protected override AIState GetInitialState()
    {
        return AIState.Idle;
    }

    protected override AIState GetStateTypeFromInstance(BaseStateAI state)
    {
        if (state is IdleState) return AIState.Idle;
        if (state is ToSiginState) return AIState.ToSigin;
        if (state is PatrolState) return AIState.Patrol;

    

        throw new Exception("Unknown state type: " + state.GetType());
    }
    private IEnumerator Init()
    {
        while (_gameplayState == null)
        {
            _gameplayState = FindAnyObjectByType<GameplayState>();
            yield return null;
        }

    }

    private void Update()
    {
        if (_gameplayState == null) return;
        if (_gameplayState.IsFinisedGame) OnPatrol = false;

            AITrigger? desiredTrigger = null;

        if (OnPatrol)
        {
            desiredTrigger = AITrigger.ToPatrol;
        }
        else if (_gameplayState.StartAi)
        {
            desiredTrigger = AITrigger.Start;
        }
        else
        {
            desiredTrigger = AITrigger.Idle;
        }


        if (desiredTrigger.HasValue && _lastTrigger != desiredTrigger.Value)
        {
            SetTrigger(desiredTrigger.Value);
            _lastTrigger = desiredTrigger.Value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            OnPatrol = true;

            if (stateMap.TryGetValue(AIState.Patrol, out var state))
            {
                if (state is PatrolState patrol)
                {
                    patrol.SetZone(other);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            OnPatrol = false;
        }
    }
    public enum AIState
    {
        Idle,
        ToSigin,
        Patrol,

    }
    public enum AITrigger
    {
        Idle,
        Start,
        ToPatrol
    }

}