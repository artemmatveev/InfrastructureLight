using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using InfrastructureLight.Wpf.Common.Dialogs;

namespace MessageBox
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick_1(object sender, RoutedEventArgs e)
        {
            MessageDialogHelper.ShowDialog("Warning MessageBox Title",
                "Warning MessageBox Message",
                MessageBoxButton.OK, 
                ResizeMode.CanResizeWithGrip, 
                MessageBoxImage.Warning);
        }

        private void ButtonBase_OnClick_2(object sender, RoutedEventArgs e)
        {
            MessageDialogHelper.ShowDialog("Information MessageBox Title",
                "Information MessageBox Message",
                MessageBoxButton.OKCancel, 
                ResizeMode.CanMinimize, 
                MessageBoxImage.Information);
        }

        private void ButtonBase_OnClick_3(object sender, RoutedEventArgs e)
        {
            MessageDialogHelper.ShowDialog("Error MessageBox Title",
                "Error MessageBox Message",
                MessageBoxButton.YesNo,
                ResizeMode.CanResize,
                MessageBoxImage.Error);
        }

        private void ButtonBase_OnClick_4(object sender, RoutedEventArgs e)
        {
            MessageDialogHelper.ShowDialog("Question MessageBox Title",
                "Question MessageBox Message",
                MessageBoxButton.YesNoCancel,
                ResizeMode.CanResize,
                MessageBoxImage.Question);            
        }
    }
}
