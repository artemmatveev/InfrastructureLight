using MahApps.Metro.Controls;
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

namespace InfrastructureLight.Wpf.Dialogs.Message
{
    /// <summary>
    /// Логика взаимодействия для MsgDialogWindow.xaml
    /// </summary>
    public partial class MsgDialogWindow : MetroWindow
    {
        public MsgDialogWindow()
        {
            InitializeComponent();
        }

        public new object Content
        {
            get { return xContentControl.Content; }
            set { xContentControl.Content = value; }
        }

        public object Image
        {
            get { return xImageContentControl.Content; }
            set { xImageContentControl.Content = value; }
        }        

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            if (button != null && button.IsDefault)
            {
                DialogResult = true;
            }
            else if (button != null && button.IsCancel)
            {
                DialogResult = false;
            }

            e.Handled = true;
            Close();
        }

        private void MessageDialogWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
                TextBlock tb = xContentControl.Content as TextBlock;
                var sv = xContentControl.Content as ScrollViewer;

                if (tb != null)
                {
                    Clipboard.SetText(tb.Text);
                }
                else if (sv != null)
                {
                    tb = sv.Content as TextBlock;

                    if (tb != null)
                    {
                        Clipboard.SetText(tb.Text);
                    }
                }
            }
        }
    }
}
