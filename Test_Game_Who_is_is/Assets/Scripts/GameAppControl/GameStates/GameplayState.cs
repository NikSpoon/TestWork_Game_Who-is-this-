using System;
using System.Collections;
using UnityEngine;

public class GameplayState : BaseGameState
{
    private bool _isReady = false;
    public override bool IsReady => _isReady;

    private Spawner _spawner;
    private Transform _banner;
    private Rigidbody _playerRb;
    private Muvment _playerMuvment;
    private CameraFollowControl _followControl;
    private GameObject _gameplayUI;
    private GameInfoUI _gameplayUIComponent;

    private int _startTimer = 5;
    private int _lookBannerTime = 3;
    private int _gameTime = 30;

    private bool _fiinishSession;

    public bool IsFinisedGame { get; protected set; }
    public bool IsRrspawnSesion { get; protected set; }

    public event Action<int, bool> Timer;
    public event Action<int, bool> StartTimer;

    public GameplayState Init(GameplayState gameplayState)
    {
        return this;
    }
    public override void Enter()
    {
        StartCoroutine(InitAll());
        IsFinisedGame = false;
        IsRrspawnSesion = false;
        _fiinishSession = false;
    }

    public override void Exit()
    {
        if (_gameplayUIComponent != null)
        {
            StartTimer -= _gameplayUIComponent.OnStartTimer;
            Timer -= _gameplayUIComponent.OnTimer;
        }
    }
    public override void GameUpdate()
    {
        if (IsRrspawnSesion)
        {
            OnFinishSesion();
        }
    }

    private void OnFinishSesion()
    {
        _fiinishSession = false;
        if (_fiinishSession)
        {
            OnFinishGame();
            return;
        }

        _gameplayUIComponent.EnebleFinishPanel(true);

    }
    private void OnFinishGame()
    {
        IsFinisedGame = false;

    }
    private IEnumerator InitAll()
    {

        while (_followControl == null)
        {
            if (Camera.main != null)
                _followControl = Camera.main.GetComponent<CameraFollowControl>();
            yield return null;
        }

        while (_playerRb == null && _playerMuvment == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _playerMuvment = player.GetComponent<Muvment>();
                _playerRb = player.GetComponent<Rigidbody>();
            }
            yield return null;
        }

        while (_banner == null)
        {
            GameObject bannerObj = GameObject.FindWithTag("BanerCameraRoot");
            if (bannerObj != null)
                _banner = bannerObj.transform;
            yield return null;
        }

        _isReady = true;

        while (_gameplayUI == null)
        {
            GameObject ui = GameObject.FindWithTag("GameUI");
            if (ui != null)
            {
                _gameplayUI = ui;
                _gameplayUIComponent = _gameplayUI.GetComponent<GameInfoUI>();
            }
            yield return null;
        }

        if (_gameplayUIComponent != null)
        {
            StartTimer += _gameplayUIComponent.OnStartTimer;
            Timer += _gameplayUIComponent.OnTimer;
            _gameplayUIComponent.SetState(this);
        }


        yield return null;

        while (_spawner == null)
        {
            GameObject spawner = GameObject.FindWithTag("Spawner");
            if (spawner != null)
                _spawner = spawner.GetComponent<Spawner>();
            yield return null;
        }

        yield return null;
 
        
        StartCoroutine(LookBanner());
    }

    private IEnumerator LookBanner()
    {
        _playerRb.isKinematic = true;
        _playerMuvment.enabled = false;
        _followControl.FollowTo(_banner);

        for (int i = 0; i < _lookBannerTime; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        _followControl?.FollowToPlayer();

        for (int i = 0; i < _startTimer; i++)
        {
            StartTimer?.Invoke(_startTimer - i, true);
            yield return new WaitForSeconds(1f);
        }
        StartTimer?.Invoke(0, false);

        _playerRb.isKinematic = false;
        _playerMuvment.enabled = true;


        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Timer?.Invoke(_gameTime, true);

        for (int i = _gameTime; i > 0; i--)
        {
            Timer?.Invoke(_gameTime - i, true);
            yield return new WaitForSeconds(1);

        }
        _fiinishSession = true;
        IsRrspawnSesion = true;
        Timer?.Invoke(0, false);
    }
    private IEnumerator RrspawnRotine()
    {

        IsRrspawnSesion = false;
        _playerMuvment.enabled = false;
        _playerRb.isKinematic = true;
        
        _gameplayUIComponent.EnebleFinishPanel(false);
        yield return null;
        
        _spawner.Respawn();
        yield return new WaitForSeconds(1);

        StartCoroutine(LookBanner());
    }
    public void RrspawnSesion()
    {
        StartCoroutine(RrspawnRotine());
    }
}

