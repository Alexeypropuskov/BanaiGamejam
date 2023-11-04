using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInstaller : MonoBehaviour
{
    private BlockRegistry _registry;
    
    public Controls Controls;
    public GameObject PausePanel;

    [Space]
    public GameObject _winPanel;
    public Button _nextGameButton;
    
    [Space]
    public GameObject _losePanel;
    public Button _restartGameButton;

    [Space]
    public TextMeshProUGUI FallBlocks;
    
    [Space]
    public int MinBlockFallForWin = 3;

    private void Awake()
    {
        Controls = new Controls();
        Controls.Enable();
        
        _registry = FindObjectOfType<BlockRegistry>();
        Controls.Mouse.Pause.performed += OnPause;
        PausePanel.SetActive(false);

        TimeManager.IsGame = true;
        
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
        _nextGameButton.onClick.AddListener(LoadNextLevel);
        _restartGameButton.onClick.AddListener(Restart);

        FallBlocks.text = $"{_registry.Falls.ToString()}/{MinBlockFallForWin}";
    }

    public void UpdateScore()
    {
        FallBlocks.text = $"{_registry.Falls.ToString()}/{MinBlockFallForWin}";
        if (_registry.Falls >= MinBlockFallForWin)
        {
            _winPanel.SetActive(true);
            TimeManager.IsGame = false;
        }
    }
    
    public void GameOver(ForcePoint point)
    {
        if (_registry.Falls >= MinBlockFallForWin)
        {
            _winPanel.SetActive(true);
            TimeManager.IsGame = false;
            return;
        }
        if (point.CurrentPower <= 0)
        {
            _losePanel.SetActive(true);
            TimeManager.IsGame = false;
            return;
        }
    }

    private void LoadNextLevel()
    {
        var count = SceneManager.sceneCount;
        var index = SceneManager.GetActiveScene().buildIndex;
        index = (index + 1) % count;
        
        PlayerPrefs.SetInt(MainMenu.c_progressKey, index);
        SceneManager.LoadScene(index);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OnPause(InputAction.CallbackContext obj)
    {
        TimeManager.IsGame = false;
        PausePanel.SetActive(true);
        foreach (var block in _registry.AllBlocks)
            block.SetFroze(true);
    }
}
