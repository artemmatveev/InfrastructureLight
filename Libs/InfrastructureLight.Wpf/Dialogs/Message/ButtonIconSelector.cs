using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InfrastructureLight.Wpf.Dialogs.Message
{
    internal static class ButtonIconSelector
    {
        public static Dictionary<ButtonIcon, BitmapImage> Icons = new Dictionary<ButtonIcon, BitmapImage>
        {
            {
                ButtonIcon.Yes,
                new BitmapImage(new Uri("pack://application:,,,/InfrastructureLight.Wpf.Common;component/Assets/Images/tick.png"))                
            },
            {
                ButtonIcon.No,
                new BitmapImage(new Uri("pack://application:,,,/InfrastructureLight.Wpf.Common;component/Assets/Images/cross.png"))                
            },
            {
                ButtonIcon.Cancel,
                new BitmapImage(new Uri("pack://application:,,,/InfrastructureLight.Wpf.Common;component/Assets/Images/prohibition.png"))                
            },
        };
    }
}
