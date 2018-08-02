using System.Windows;
using Autofac;

namespace DialogVisualizer
{
    using ViewModels;

    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            DataContext = Bootstrapper.Container.Resolve<MainViewModel>();            
            InitializeComponent();
        }        
    }
}
