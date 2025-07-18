using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishState : BaseGameState
{
    private bool _isReady = false;

    public override bool IsReady => _isReady;
    public override void Enter()
    {
     
    }

    public override void Exit()
    {
      
    }

    public override void AppUpdate()
    {
        // логика игры
    }
}

