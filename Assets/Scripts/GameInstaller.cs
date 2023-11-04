using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    private BlockRegistry _registry;
    
    public Controls Controls;

    public Button PauseButton; 
    public GameObject PausePanel;
    
    public override void InstallBindings()
    {
        _registry = FindObjectOfType<BlockRegistry>();
        PauseButton.onClick.AddListener(OnPause);
        PausePanel.SetActive(false);
        
        Controls = new Controls();
        Controls.Enable();
        Container.BindInstance(Controls).AsSingle();
        TimeManager.IsGame = true;
    }

    public void OnPause()
    {
        TimeManager.IsGame = false;
        PausePanel.SetActive(true);
        foreach (var block in _registry.AllBlocks)
            block.SetFroze(true);
    }
}
