using TMPro;
using UnityEditor;
using UnityEngine;

public class GameInfoUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _startTimer;
    [SerializeField] private TextMeshProUGUI _gameTimer;

    [Header("EndPAnel")]
    [SerializeField] private TextMeshProUGUI _finishTeet;
    [SerializeField] private GameObject _finishPanel;

    private bool isStartTimer = false;

    private bool isGameTimer = false;

    private GameplayState _state;

    public void SetState(GameplayState state)
    {
        _state = state;
    }
    public void OnStartTimer(int timeLeft, bool running)
    {
        isStartTimer = running;
        _startTimer.text = $"Start after - {timeLeft}";
    }

    public void OnTimer(int timeLeft, bool running)
    {
        isGameTimer = running;
        _gameTimer.text = $"Start after - {timeLeft}";
    }
    private void Update()
    {

        _startTimer.gameObject.SetActive(isStartTimer);
        _gameTimer.gameObject.SetActive(isGameTimer);

    }
    public void EnebleFinishPanel(bool t)
    {
        _finishPanel.SetActive(t);

        _finishTeet.text = "Thanks for playing! " +
                      "\r\n Come back again soon!";
    }
 
    public void OnMenu()
    {
        GameManager.Instance.Trigger(AppTriger.ToMainMenu);
    }
    public void OnRestart()
    {
        if (_state.IsFinisedGame)
        {
            GameManager.Instance.Trigger(AppTriger.ToGameplay);
        }
        else if (_state.IsRrspawnSesion)
        {
            _state.RrspawnSesion();
        }
    }
    public void OnPay()
    {

    }
}
