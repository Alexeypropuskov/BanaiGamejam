using Zenject;

public class GameInstaller : MonoInstaller
{
    public Controls Controls;
    
    public override void InstallBindings()
    {
        Controls = new Controls();
        Controls.Enable();
        Container.BindInstance(Controls).AsSingle();
    }
}
