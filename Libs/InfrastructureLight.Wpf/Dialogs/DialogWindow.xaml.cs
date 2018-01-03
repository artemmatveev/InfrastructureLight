using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace InfrastructureLight.Wpf.Dialogs
{    
    public partial class DialogWindow : MetroWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        #region Public

        public new object Content
        {
            get { return xContentControl.Content; }
            set { xContentControl.Content = value; }
        }
        
        #endregion        
    }
}
