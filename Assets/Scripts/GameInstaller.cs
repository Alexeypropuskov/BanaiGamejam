using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInstaller : MonoBehaviour
{
    private bool _finish;
    private BlockRegistry _registry;
    private Camera _camera;
    private Transform _cameraTransform;
    public Comics _comics;
    public Comics _lastComics;
    [HideInInspector]
    public Target Target;
    
    public Controls Controls;
    public GameObject PausePanel;

    [Space]
    public GameObject _winPanel;
    public Button _nextGameButton;
    
    [Space]
    public GameObject _losePanel;
    public GameObject _loseBacked;
    public Button _restartGameButton;

    [Space]
    public TextMeshProUGUI FallBlocks;
    
    [Space]
    //public int MinBlockFallForWin = 3;
    public float LoseDelay = 5f;

    [Header("---Movement---")]
    [Range(0f, 100f)]
    public float MoveSpeed = 3f;

    [Header("---Sound---")]
    public AudioClip Soundtrack;
    
    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        Target = FindObjectOfType<Target>();
        _cameraTransform = _camera.transform;
        Controls = new Controls();
        Controls.Enable();
        
        _registry = FindObjectOfType<BlockRegistry>();
        Controls.Mouse.Pause.performed += OnPause;
        //Controls.Mouse.Right.performed += OnRight;
        //Controls.Mouse.Right.canceled += StopRight;
        PausePanel.SetActive(false);

        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
        _loseBacked.SetActive(false);
        _nextGameButton.onClick.AddListener(LoadNextLevel);
        _restartGameButton.onClick.AddListener(Restart);

        //FallBlocks.text = $"{_registry.Falls.ToString()}/{MinBlockFallForWin}";
    }

    private void Start()
    {
        if (_comics != null)
        {
            TimeManager.IsGame = false;
            foreach (var block in _registry.AllBlocks)
                block.SetFroze(true);
            _comics.Show();
            _comics.CloseButton.onClick.AddListener(CloseComics);
        }
        else
            TimeManager.IsGame = true;
        AudioManager.SetSoundtrack(Soundtrack);
    }

    public void UpdateScore()
    {/*
        FallBlocks.text = $"{_registry.Falls.ToString()}/{MinBlockFallForWin}";
        if (_registry.Falls >= MinBlockFallForWin)
        {
            _winPanel.SetActive(true);
            TimeManager.IsGame = false;
        }*/
    }

    public void Win()
    {
        if (_finish) return;
        _finish = true;
        if (_registry.Falls >= Target.CountNeedToFall())
        {
            _winPanel.SetActive(true);
            AudioManager.PlayEventWin();
            TimeManager.IsGame = false;
            
            if(_lastComics != null)
            {
                foreach (var block in _registry.AllBlocks)
                    block.SetFroze(true);
                _lastComics.Show();
                _lastComics.CloseButton.onClick.AddListener(LoadNextLevel);
            }
        }
    }
    
    public void Lose()
    {
        if (_finish) return;
        _finish = true;
        StartCoroutine(LoseDelayGame());
    }

    private void LateUpdate()
    {
        if (!TimeManager.IsGame) return;

        var target = Target.transform.position;
        target.x += OffsetTarget;
        var view = _camera.WorldToViewportPoint(target);
        if (view.x is >= 0 and <= 1 && view.y is >= 0 and <= 1 && view.z > 0) { }
        else
        {
            var pos = _cameraTransform.position;
            pos.x += MoveSpeed * TimeManager.DeltaTime;
            _cameraTransform.position = pos;
            //if (_move != null)
           //     StopCoroutine(_move);
            //_move = StartCoroutine(Move());
        }
    }

    private Coroutine _move;
    public float OffsetTarget = 5f;
    
    private IEnumerator Move()
    {
        var target = _cameraTransform.position;
        target.x = Target.transform.position.x;
        var time = 0f;
        while (MoveSpeed > time)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, target, 1f - (MoveSpeed - time) / MoveSpeed);
            time += TimeManager.DeltaTime;
            yield return null;
        }
			
        _move = null;
    }
    
    //private void OnRight(InputAction.CallbackContext a) => _move = true;
    ///private void StopRight(InputAction.CallbackContext a) => _move = false;
    
    private IEnumerator LoseDelayGame()
    {
        AudioManager.PlayEventNoEnergy();
        _losePanel.SetActive(true);
        Controls.Disable();
        
        yield return new WaitForSeconds(LoseDelay);
        _loseBacked.SetActive(true);
        AudioManager.PlayEventLose();
        TimeManager.IsGame = false;
    }
    
    private void LoadNextLevel()
    {
        AudioManager.PlayEventClick();
        var count = SceneManager.sceneCount;
        var index = SceneManager.GetActiveScene().buildIndex;
        index = (index + 1) % count;
        
        PlayerPrefs.SetInt(MainMenu.c_progressKey, index);
        SceneManager.LoadScene(index);
    }

    private void Restart()
    {
        AudioManager.PlayEventClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OnPause(InputAction.CallbackContext obj)
    {
        TimeManager.IsGame = false;
        PausePanel.SetActive(true);
        foreach (var block in _registry.AllBlocks)
            block.SetFroze(true);
    }

    public void CloseComics()
    {
        TimeManager.IsGame = true;
        foreach (var block in _registry.AllBlocks)
            block.SetFroze(false);
    }
}
