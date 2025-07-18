using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishState : BaseGameState
{
    private bool _isReady = false;

    public override bool IsReady => _isReady;

    public override void Enter()
    {
        _isReady = true;
    }

    public override void Exit()
    {
      
    }

    public override void AppUpdate()
    {
       
    }
}

