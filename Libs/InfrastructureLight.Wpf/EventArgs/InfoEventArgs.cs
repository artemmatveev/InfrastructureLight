using System;

namespace InfrastructureLight.Wpf.EventArgs
{
    public class InfoEventArgs : System.EventArgs
    {
        public string Message { get; private set; }
        public Action Callback { get; private set; }

        public InfoEventArgs(string message, Action callback)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (callback == null) throw new ArgumentNullException(nameof(callback));

            Callback = callback;
            Message = message;
        }
    }
}
