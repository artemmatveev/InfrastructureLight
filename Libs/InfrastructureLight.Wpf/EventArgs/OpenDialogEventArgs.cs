using System;

namespace InfrastructureLight.Wpf.EventArgs
{
    public class OpenDialogEventArgs<T> : System.EventArgs where T : class
    {
        public T DataContext { get; private set; }
        public Action<T> Callback { get; private set; }

        public OpenDialogEventArgs(T dataContext)
            : this(dataContext, null)
        {

        }

        public OpenDialogEventArgs(T dataContext, Action<T> callback)
        {
            DataContext = dataContext;
            Callback = callback;
        }
    }
}
