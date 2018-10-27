using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro;
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

        public void ChangeAppStyle(string color)
        {
            ThemeManager.ChangeAppStyle(this, ThemeManager.GetAccent(color), 
                ThemeManager.GetAppTheme("BaseLight"));
        }

        public new object Content
        {
            get { return xContentControl.Content; }
            set { xContentControl.Content = value; }
        }
        
        #endregion        
    }
}
