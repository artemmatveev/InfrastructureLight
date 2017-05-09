namespace InfrastructureLight.Wpf.Factory
{
    using ViewModels;

    public interface IViewModelFactory
    {
        T Get<T>() where T : ViewModelBase;
        T Create<T>() where T : ViewModelBase;
    }
}
