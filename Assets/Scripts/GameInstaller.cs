using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public Controls Controls;

    public Button PauseButton; 
    public GameObject PausePanel;
    
    public override void InstallBindings()
    {
        PauseButton.onClick.AddListener(OnPause);
        PausePanel.SetActive(false);
        
        Controls = new Controls();
        Controls.Enable();
        Container.BindInstance(Controls).AsSingle();
    }

    public void OnPause()
    {
        TimeManager.IsGame = false;
        PausePanel.SetActive(true);
    }
}
