using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InfrastructureLight.Wpf.ViewModels
{
    using Commands;
    using ComponentModel;
    using EventArgs;

    public abstract class ViewModelBase : NotifyPropertyEntity, IDataErrorInfo
    {                
        protected ViewModelBase()
        {
            SaveCommand = new DelegateCommand(action => Apply(), action => CanApply());
            CancelCommand = new DelegateCommand(action => Cancel(), action => CanCancel());
        }

        #region IDataErrorInfo, Validation Logic

        readonly Dictionary<string, List<Binder>> ruleMap = new Dictionary<string, List<Binder>>();

        public void AddRule<T>(Expression<Func<T>> propertyExpression, Func<bool> ruleDelegate, string errorMessage)
        {
            var propertyName = GetPropertyName(propertyExpression);
            this.AddRule(propertyName, ruleDelegate, errorMessage);
        }

        public void AddRule(string propertyName, Func<bool> ruleDelegate, string errorMessage)
        {
            if (ruleMap.Any(r => r.Key == propertyName))
            {
                ruleMap[propertyName].Add(new Binder(ruleDelegate, errorMessage));
            }
            else
            {
                ruleMap.Add(propertyName, new List<Binder>() { new Binder(ruleDelegate, errorMessage) });
            }
        }

        public bool HasErrors
        {
            get
            {
                var values = ruleMap.Values.SelectMany(v => v).ToList();
                values.ForEach(b => b.Update());

                return values.Any(b => b.HasError);
            }
        }

        public string Error
        {
            get
            {
                var errors = from b in ruleMap.Values.SelectMany(v => v).ToList()
                             where b.HasError
                             select b.Error;

                return string.Join("\n", errors);
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (ruleMap.ContainsKey(columnName))
                {
                    foreach (var value in ruleMap[columnName])
                    {
                        value.Update();
                        if (value.HasError)
                        {
                            return value.Error;
                        }
                    }
                }
                return string.Empty;
            }
        }

        private class Binder
        {
            private readonly Func<bool> _ruleDelegate;
            private readonly string _message;

            internal Binder(Func<bool> ruleDelegate, string message)
            {
                this._ruleDelegate = ruleDelegate;
                this._message = message;
            }

            internal string Error { get; set; }
            internal bool HasError { get; set; }

            internal void Update()
            {
                Error = null;
                HasError = false;
                try
                {
                    if (!_ruleDelegate())
                    {
                        Error = _message;
                        HasError = true;
                    }
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    HasError = true;
                }
            }
        }

        private string GetPropertyName<T>(Expression<Func<T>> exp)
        {
            return (((MemberExpression)(exp.Body)).Member).Name;
        }

        #endregion

        #region Fields

        string _title;
        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChangedEvent(); }
        }

        public bool? DialogResult { get; set; }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        protected virtual void Apply()
        {
            DialogResult = true;
            OnApplied();
        }

        protected virtual bool CanApply() => true;

        public ICommand CancelCommand { get; }
        protected virtual void Cancel()
        {
            DialogResult = false;
            OnCanceled(false);
        }

        protected virtual bool CanCancel() => true;

        #endregion

        #region Events

        private EventHandler _appliedInvocList;
        public event EventHandler Applied
        {
            add
            {
                if (_appliedInvocList == null || _appliedInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _appliedInvocList += value;
                }
            }
            remove { _appliedInvocList -= value; }
        }
        protected virtual void OnApplied()
        {
            EventHandler handler = _appliedInvocList;
            if (handler != null) handler(this, System.EventArgs.Empty);
        }

        private EventHandler<CancelDialogEventArgs> _canceledInvocList;
        public event EventHandler<CancelDialogEventArgs> Canceled
        {
            add
            {
                if (_canceledInvocList == null || _canceledInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _canceledInvocList += value;
                }
            }
            remove { _canceledInvocList -= value; }
        }
        protected virtual void OnCanceled(bool dialogResult)
        {
            EventHandler<CancelDialogEventArgs> handler = _canceledInvocList;
            if (handler != null) handler(this, new CancelDialogEventArgs(dialogResult));
        }

        private EventHandler<ConfirmEventArgs> _confirmInvocList;
        public event EventHandler<ConfirmEventArgs> Confirm
        {
            add
            {
                if (_confirmInvocList == null || _confirmInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _confirmInvocList += value;
                }
            }
            remove { _confirmInvocList -= value; }
        }
        protected virtual void OnConfirm(string message, Action callback)
        {
            EventHandler<ConfirmEventArgs> handler = _confirmInvocList;
            if (handler != null) handler(this, new ConfirmEventArgs(message, callback));
        }

        private EventHandler<FailureEventArgs> _failureInvocList;
        public event EventHandler<FailureEventArgs> Failure
        {
            add
            {
                if (_failureInvocList == null || _failureInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _failureInvocList += value;
                }
            }
            remove { _failureInvocList -= value; }
        }
        protected virtual void OnFailure(string message, Action callback)
        {
            EventHandler<FailureEventArgs> handler = _failureInvocList;
            if (handler != null) handler(this, new FailureEventArgs(message, callback));
        }

        private EventHandler<InfoEventArgs> _infoInvocList;
        public event EventHandler<InfoEventArgs> Info
        {
            add
            {
                if (_infoInvocList == null || _infoInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _infoInvocList += value;
                }
            }
            remove { _infoInvocList -= value; }
        }
        protected virtual void OnInfo(string message, Action callback)
        {
            EventHandler<InfoEventArgs> handler = _infoInvocList;
            if (handler != null) handler(this, new InfoEventArgs(message, callback));
        }

        #endregion

        #region Methods

        protected bool SetValue<T>(ref T field, T value = default(T), [CallerMemberName] string propertyName = null)
        {
            bool equalsFlag = EqualityComparer<T>.Default.Equals(field, value);
            if (!equalsFlag)
            {
                field = value;
                RaisePropertyChangedEvent(propertyName);
            }
            return !equalsFlag;
        }

        public virtual void OnClosed()
        {
        }

        #endregion
    }
}
