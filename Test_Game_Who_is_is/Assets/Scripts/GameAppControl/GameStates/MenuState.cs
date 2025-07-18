using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState : BaseGameState
{
    private bool _isReady = false;

    public override bool IsReady => _isReady;
    public override void Enter()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public override void Exit()
    {
       
    }

    public override void AppUpdate()
    {
        // логика игры
    }
}
