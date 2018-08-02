using System.Windows.Input;

using InfrastructureLight.Wpf.Commands;
using InfrastructureLight.Wpf.Dialogs;
using InfrastructureLight.Wpf.Factory;
using InfrastructureLight.Wpf.ViewModels;

namespace DialogVisualizer.ViewModels
{
    public class StaffUnitViewModel : ViewModelBase {
        
        readonly IDialogVisualizer _dialogVisualizer;
        readonly IViewModelFactory _viewModelFactory;

        public StaffUnitViewModel(IDialogVisualizer dialogVisualizer, IViewModelFactory viewModelFactory)
        {
            _dialogVisualizer = dialogVisualizer;
            _viewModelFactory = viewModelFactory;

            ShowDialog2Command = new DelegateCommand(arg => ShowDialog());
        }

        public ICommand ShowDialog2Command { get; private set; }

        private void ShowDialog()
        {
            var vm = _viewModelFactory.Create<UserViewModel>();
            _dialogVisualizer.ShowDialog(vm,
                this,
                new DialogSettings()
                {                    
                    DialogWidth = 200,
                    DialogHeight = 800
                }, _ =>
                {
                    // TODO: you code after close dialog
                });
        }
    }
}
