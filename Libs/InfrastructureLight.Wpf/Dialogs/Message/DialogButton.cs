using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InfrastructureLight.Wpf.Dialogs.Message
{
    public class DialogButton
    {
        public DialogButton(string content, ICommand command, ButtonIcon? icon = null, bool closeWindow = true, bool isDefault = false, bool isCancel = false)
        {
            Content = content;
            Command = command;
            Icon = icon;
            CloseWindow = closeWindow;
            IsDefault = isDefault;
            IsCancel = isCancel;
        }

        public string Content { get; }
        public ICommand Command { get; }
        public ButtonIcon? Icon { get; }
        public bool CloseWindow { get; }
        public bool IsDefault { get; }
        public bool IsCancel { get; }
    }
}
