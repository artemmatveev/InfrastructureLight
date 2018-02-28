using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLight.Wpf.Dialogs
{
    public interface IDialogSettings
    {
        /// <summary>
        ///     Заголовок диалога
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Ширина диалога
        /// </summary>
        double DialogWidth { get; set; }

        /// <summary>
        ///     Высота диалога
        /// </summary>
        double DialogHeight { get; set; }
    }
}
