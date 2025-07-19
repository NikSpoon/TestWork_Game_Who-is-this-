using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingState : BaseGameState
{
    private bool _isReady = false;


    public override bool IsReady => _isReady;

    public override void Enter()
    {
        gameObject.SetActive(true);
        _isReady = true;
    }

    public override void Exit()
    {
        gameObject.SetActive(false);
    }

    public override void GameUpdate()
    {
        gameObject.SetActive(true);
    }
}

