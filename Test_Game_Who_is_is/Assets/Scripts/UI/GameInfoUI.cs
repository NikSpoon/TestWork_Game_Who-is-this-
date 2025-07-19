using TMPro;
using UnityEngine;

public class GameInfoUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _startTimer;
    [SerializeField] private TextMeshProUGUI _gameTimer;

    [Header("EndPAnel")]
    [SerializeField] private TextMeshProUGUI _teet;
    [SerializeField] private GameObject _finishPanel;

    private bool isStartTimer = false;

    private bool isGameTimer = false;
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
}
