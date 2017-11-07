using System;
using System.Collections.Generic;
using System.Globalization;
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

using InfrastructureLight.Common.Extensions;

namespace InfrastructureLight.Wpf.Common.Controls
{
    /// <summary>
    ///     Логика взаимодействия для FilesView.xaml
    /// </summary>
    public partial class FilesView : UserControl
    {
        public FilesView()
        {
            InitializeComponent();
        }

        public string ItemsSourceIsEmptyText
        {
            set
            {
                this.xItemsSourceIsEmptyText.Text = value;
            }
        }

    }
    
    #region Converters

    public class FileNameToIconFileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ext = System.IO.Path.GetExtension((string)value).Replace('.', ' ').Trim().ToLower();

            if (ext.In("rtf", "docx")) { ext = "doc"; }
            else if (ext.In("jpg", "jp2", "jpeg", "gif", "bmp", "tiff", "tif")) { ext = "png"; }
            else if (ext.In("htm")) { ext = "html"; }
            else if (ext.In("xlsx")) { ext = "xls"; }
            else if (ext.In("pptx")) { ext = "ppt"; }
            else if (ext.In("rar", "7z")) { ext = "zip"; }
            else if (ext.In("log")) { ext = "txt"; }
            else { ext = "unknown"; }

            return "pack://application:,,,/InfrastructureLight.Wpf.Common;component/Assets/Images/FileTypes/file_{0}.png".f(ext);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
