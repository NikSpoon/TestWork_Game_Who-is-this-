using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState : BaseGameState
{
    private bool _isReady = false;
    

    public override bool IsReady => _isReady;

    public override void Enter()
    {
        MusicManager.Instance.PlayMainTheme1();
        _isReady = true;
    }

    public override void Exit()
    {
     
    }

    public override void GameUpdate()
    {
       
    }
}
