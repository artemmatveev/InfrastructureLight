using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace InfrastructureLight.Wpf.Common.Dialogs
{    
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
    }
}
