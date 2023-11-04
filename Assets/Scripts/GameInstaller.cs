using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    private BlockRegistry _registry;
    
    public Controls Controls;
    public GameObject PausePanel;
    
    public override void InstallBindings()
    {
        _registry = FindObjectOfType<BlockRegistry>();
        Controls.Mouse.Pause.performed += OnPause;
        PausePanel.SetActive(false);
        
        Controls = new Controls();
        Controls.Enable();
        Container.BindInstance(Controls).AsSingle();
        TimeManager.IsGame = true;
    }

    private void OnPause(InputAction.CallbackContext obj)
    {
        TimeManager.IsGame = false;
        PausePanel.SetActive(true);
        foreach (var block in _registry.AllBlocks)
            block.SetFroze(true);
    }
}
