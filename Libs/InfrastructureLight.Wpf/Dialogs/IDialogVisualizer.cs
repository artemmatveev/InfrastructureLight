using System;
namespace InfrastructureLight.Wpf.Dialogs
{
    using ViewModels;

    public interface IDialogVisualizer
    {
        void Show<T>(T viewModel, Action<T> callback = null) where T : ViewModelBase;
        void Show<T, TOwner>(T viewModel, TOwner owner, Action<T> callback = null) where T : ViewModelBase where TOwner : ViewModelBase;
        bool? ShowDialog<T>(T viewModel, Action<T> callback = null) where T : ViewModelBase;
        bool? ShowDialog<T, TOwner>(T viewModel, TOwner owner, Action<T> callback = null) where T : ViewModelBase where TOwner : ViewModelBase;        
    }
}
