using UnityEngine;
using UnityEngine.SceneManagement;

public class FInishState : BaseGameState
{
    private bool _isReady = false;

    public override bool IsReady => _isReady;
    public override void Enter()
    {
        Debug.Log("Gameplay Enter");
        SceneManager.LoadScene("GameplayScene");
    }

    public override void Exit()
    {
        Debug.Log("Gameplay Exit");
        // Выгрузи что надо
    }

    public override void AppUpdate()
    {
        // логика игры
    }
}

