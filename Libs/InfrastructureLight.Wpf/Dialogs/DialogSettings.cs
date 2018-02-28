using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLight.Wpf.Dialogs
{
    public class DialogSettings : IDialogSettings
    {        
        public string Title { get; set; }
        public double DialogWidth { get; set; }
        public double DialogHeight { get; set; }
    }
}
