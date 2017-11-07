using System.Linq;
using System.Windows;

using MahApps.Metro.Controls;

namespace InfrastructureLight.Wpf.Common.Helpers
{
    public class FlyoutHelper
    {
        public static Flyout ActiveFlyout(int index)
        {
            var mWindow = Application.Current.Windows.OfType<Window>()
                .SingleOrDefault(x => x.IsActive) as MetroWindow;

            var flyout = mWindow.Flyouts.Items[index] as Flyout;

            if (flyout == null)
                return null;

            flyout.IsOpen = !flyout.IsOpen;

            return flyout;
        }
    }
}
