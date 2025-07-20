using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class GameplayState : BaseGameState
{
    private bool _isReady = false;
    public override bool IsReady => _isReady;

    private GameObject _player;
    private Spawner _spawner;
    private Transform _banner;
    private Rigidbody _playerRb;
    private Muvment _playerMuvment;
    private CameraFollowControl _followControl;
    private GameObject _gameplayUI;
    private GameInfoUI _gameplayUIComponent;
    private MemController _memController;
    private MemSpawner _memSpawner;

    private int _startTimer = 5;
    private int _lookBannerTime = 3;
    private int _gameTime = 30;

    private bool _fiinishSession;

    public bool IsFinisedGame { get; protected set; }
    public bool IsRrspawnSesion { get; protected set; }
    public bool StartAi { get; private set; }



    public event Action<int, bool> Timer;
    public event Action<int, bool> StartTimer;

    private void Awake()
    {
        StartAi = false;
    }
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
     
        MusicManager.Instance.PlayMainTheme2();
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
        if (_fiinishSession)
        {
            bool playerWonRound = _memController.GetPlayerChoice(_player);
            _fiinishSession = false;

            if (!playerWonRound)
            {
                StartAi = false;
                OnFinishGame(); 
            }
            else
            {
                HandlePostRound(); 
            }
        }
    }

    private void HandlePostRound()
    {
        if (CheckPlayerWin())
        {
            OnWinGame();
        }
        else
        {
            ContinueSesion();
        }
    }
    private void OnWinGame()
    {
        IsFinisedGame = true;
        _gameplayUIComponent.EnebleWinPanel(true);
    }

    private void OnFinishGame()
    {
        IsFinisedGame = true;
        _gameplayUIComponent.EnebleFinishPanel(true);
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
            _player = GameObject.FindWithTag("Player");
            if (_player != null)
            {
                _playerMuvment = _player.GetComponent<Muvment>();
                _playerRb = _player.GetComponent<Rigidbody>();
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

        while (_memController == null)
        {
            GameObject mc = GameObject.FindWithTag("MemController");
            if (mc != null)
            {
                _memController = mc.GetComponent<MemController>();
                _memSpawner = mc.GetComponent<MemSpawner>();
            }
            List<GameObject> participants = new List<GameObject>(_spawner.SpawnPlayers.Keys);
          
            _memController.InitParticipants(participants);
            _memSpawner.SpawnMem();
            
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
       
        StartAi = true;
        Timer?.Invoke(_gameTime, true);

        for (int i = 0; i < _gameTime; i++)
        {
            Timer?.Invoke(_gameTime - i, true);
            yield return new WaitForSeconds(1);
        }
        
        AudioClip memMusic = _memSpawner.GetMusic();
        if (memMusic != null)
        {
            MusicManager.Instance.PlayOneShot(memMusic);
            yield return new WaitForSeconds(memMusic.length);
        }
       
        _fiinishSession = true;
        IsRrspawnSesion = true;
        StartAi = false;
        Timer?.Invoke(0, false);
       
        StartCoroutine(WaitForMusicToEnd());
    }
    private IEnumerator WaitForMusicToEnd()
    {
        while (MusicManager.Instance != null && MusicManager.Instance.IsPlaying())
        {
            yield return null;
        }
       MusicManager.Instance. PlayMainTheme2();
    }
    private IEnumerator ContinueWithSurvivorsRoutine()
    {
        IsRrspawnSesion = false;
        _playerMuvment.enabled = false;
        _playerRb.isKinematic = true;

        _gameplayUIComponent.EnebleFinishPanel(false);
        yield return null;
        
        _memSpawner.ClearPrevious();
        _spawner.Respawn();
        _memSpawner.SpawnMem();
        yield return new WaitForSeconds(1);

        _memController.InitParticipants(new List<GameObject>(_spawner.SpawnPlayers.Keys));

        StartCoroutine(LookBanner());
    }
    public void ContinueSesion()
    {
        StartAi = false;
        _memController.DisableLosers();

        List<GameObject> toRemove = new List<GameObject>();
        foreach (var kvp in _spawner.SpawnPlayers)
        {
            if (!kvp.Key.activeSelf)
            {
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var dead in toRemove)
        {
            _spawner.SpawnPlayers.Remove(dead);
        }

        StartCoroutine(ContinueWithSurvivorsRoutine());
    }
    private bool CheckPlayerWin()
    {
        int activePlayersCount = _memController.CheckPlayers();

        return activePlayersCount == 1;
    }
}

