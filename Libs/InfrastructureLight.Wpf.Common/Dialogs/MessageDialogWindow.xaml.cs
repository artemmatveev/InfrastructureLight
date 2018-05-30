using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace InfrastructureLight.Wpf.Common.Dialogs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public partial class MessageDialogWindow : MetroWindow
    {
        public MessageDialogWindow()
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
              
        private void OnClose(object sender, ExecutedRoutedEventArgs e)
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
