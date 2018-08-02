using System.Windows.Input;

using InfrastructureLight.Wpf.Commands;
using InfrastructureLight.Wpf.Dialogs;
using InfrastructureLight.Wpf.Factory;
using InfrastructureLight.Wpf.ViewModels;

namespace DialogVisualizer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        readonly IDialogVisualizer _dialogVisualizer;
        readonly IViewModelFactory _viewModelFactory;

        public MainViewModel(IDialogVisualizer dialogVisualizer, IViewModelFactory viewModelFactory)
        {
            _dialogVisualizer = dialogVisualizer;
            _viewModelFactory = viewModelFactory;

            ShowDialog1Command = new DelegateCommand(arg => ShowDialog());
        }

        public ICommand ShowDialog1Command { get; private set; }

        private void ShowDialog()
        {
            var vm = _viewModelFactory.Create<StaffUnitViewModel>();
            _dialogVisualizer.ShowDialog(vm,                
                this,
                new DialogSettings()
                {
                    Title = "Dialog 1",
                    DialogWidth = 500,
                    DialogHeight = 500
                }, _ =>
                {
                    // TODO: you code after close dialog
                });
        }
    }
}
