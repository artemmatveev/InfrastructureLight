using System;
using System.Threading.Tasks;

namespace InfrastructureLight.Wpf.ViewModels
{
    public abstract class AsyncViewModel : ViewModelBase
    {
        #region Fields

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            protected set { _busy = value; RaisePropertyChangedEvent("Busy"); }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Executes a given action asynchronously.
        /// </summary>
        /// <param name="action">A delegate to invoke asynchronously.</param>
        /// <param name="onSuccess">A delegate to invoke upon successful execution.</param>
        /// <param name="onFailure">A delegate to invoke upon failure.</param>
        protected void Go(Action action, Action onSuccess = null, Action<Exception> onFailure = null)
        {
            var task = new Task(action);

            task.ContinueWith(t =>
            {
                Busy = false;

                if (t.IsFaulted)
                {
                    if (onFailure != null)
                    {
                        onFailure(t.Exception);
                    }
                    else
                    {
                        OnFailure(t.Exception);
                    }

                    return;
                }

                if (onSuccess != null)
                {
                    onSuccess();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Busy = true;
            task.Start();
        }

        /// <summary>
        ///     Handles exceptions during execution when no explicit handler given.
        /// </summary>
        /// <param name="exception">An instance of <see cref="AggregateException" /> representing errors occured.</param>
        protected virtual void OnFailure(AggregateException exception)
        {
            throw exception;
        }

        /// <summary>
        ///     Executes a given action asynchronously.
        /// </summary>
        /// <param name="action">A delegate to invoke asynchronously.</param>
        /// <param name="parameter">Parameter #1.</param>
        /// <param name="onSuccess">A delegate to invoke upon successful execution.</param>
        /// <param name="onFailure">A delegate to invoke upon failure.</param>
        protected void Go<T>(Action<T> action, T parameter, Action onSuccess = null, Action<Exception> onFailure = null)
        {
            Go(() => action(parameter), onSuccess, onFailure);
        }

        /// <summary>
        ///     Executes a given action asynchronously.
        /// </summary>
        /// <param name="action">A delegate to invoke asynchronously.</param>
        /// <param name="parameter1">Parameter #1.</param>
        /// <param name="parameter2">Parameter #2.</param>
        /// <param name="onSuccess">A delegate to invoke upon successful execution.</param>
        /// <param name="onFailure">A delegate to invoke upon failure.</param>
        protected void Go<T1, T2>(Action<T1, T2> action, T1 parameter1, T2 parameter2, Action onSuccess = null, Action<Exception> onFailure = null)
        {
            Go(() => action(parameter1, parameter2), onSuccess, onFailure);
        }

        /// <summary>
        ///     Executes a given action asynchronously.
        /// </summary>
        /// <param name="action">A delegate to invoke asynchronously.</param>
        /// <param name="parameter1">Parameter #1.</param>
        /// <param name="parameter2">Parameter #2.</param>
        /// <param name="parameter3">Parameter #3.</param>
        /// <param name="onSuccess">A delegate to invoke upon successful execution.</param>
        /// <param name="onFailure">A delegate to invoke upon failure.</param>
        protected void Go<T1, T2, T3>(Action<T1, T2, T3> action, T1 parameter1, T2 parameter2, T3 parameter3, Action onSuccess = null, Action<Exception> onFailure = null)
        {
            Go(() => action(parameter1, parameter2, parameter3), onSuccess, onFailure);
        }

        #endregion
    }
}
