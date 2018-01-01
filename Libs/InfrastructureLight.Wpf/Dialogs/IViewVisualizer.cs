namespace InfrastructureLight.Wpf.Dialogs
{
    using ViewModels;

    public interface IViewVisualizer
    {
        void Show<T>(T viewModel) where T : ViewModelBase;
        void Show<T, TOwner>(T viewModel, TOwner owner) where T : ViewModelBase where TOwner : ViewModelBase;
        bool? ShowDialog<T>(T viewModel) where T : ViewModelBase;
        bool? ShowDialog<T, TOwner>(T viewModel, TOwner owner) where T : ViewModelBase where TOwner : ViewModelBase;
        void Close<T>(T viewModel) where T : ViewModelBase;
    }
}
