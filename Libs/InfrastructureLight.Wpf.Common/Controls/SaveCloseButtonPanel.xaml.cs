using System.Windows.Controls;

namespace InfrastructureLight.Wpf.Common.Controls
{    
    public partial class SaveCloseButtonPanel : UserControl
    {
        public SaveCloseButtonPanel()
        {
            InitializeComponent();
        }

        public string SaveText
        {
            set 
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.xSaveButton.Content = value;
                }
            }
        }

        public string CloseText
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.xCloseButton.Content = value;
                }
            }
        }
    }
}
