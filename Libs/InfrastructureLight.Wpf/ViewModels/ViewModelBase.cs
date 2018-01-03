using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace InfrastructureLight.Wpf.ViewModels
{
    using Commands;
    using EventArgs;

    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        readonly ICommand _saveCommand;
        readonly ICommand _closeCommand;

        public ViewModelBase()
        {
            _saveCommand = new DelegateCommand(action => Save(), action => CanSave());
            _closeCommand = new DelegateCommand(action => Close(), action => CanClose());
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                VerifyPropertyName(propertyName);
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Не сущесвует свойство с именем: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion

        #region IDataErrorInfo, Validation Logic

        private Dictionary<string, List<Binder>> ruleMap = new Dictionary<string, List<Binder>>();

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
            private readonly Func<bool> ruleDelegate;
            private readonly string message;

            internal Binder(Func<bool> ruleDelegate, string message)
            {
                this.ruleDelegate = ruleDelegate;
                this.message = message;
            }

            internal string Error { get; set; }
            internal bool HasError { get; set; }

            internal void Update()
            {
                Error = null;
                HasError = false;
                try
                {
                    if (!ruleDelegate())
                    {
                        Error = message;
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
            get { return _title; }
            set { _title = value; RaisePropertyChangedEvent(); }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand => _saveCommand;
        protected virtual void Save()
        {
            OnSaved();
        }
        protected virtual bool CanSave()
        {
            return true;
        }

        public ICommand CloseCommand => _closeCommand;
        protected virtual void Close()
        {
            OnClosed(false);
        }
        protected virtual bool CanClose()
        {
            return true;
        }

        #endregion

        #region Events

        private EventHandler _savedInvocList;
        public event EventHandler Saved
        {
            add
            {
                if (_savedInvocList == null || _savedInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _savedInvocList += value;
                }
            }
            remove { _savedInvocList -= value; }
        }
        protected virtual void OnSaved()
        {
            EventHandler handler = _savedInvocList;
            if (handler != null) handler(this, System.EventArgs.Empty);
        }

        private EventHandler<CloseDialogEventArgs> _closedInvocList;
        public event EventHandler<CloseDialogEventArgs> Closed
        {
            add
            {
                if (_closedInvocList == null || _closedInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _closedInvocList += value;
                }
            }
            remove { _closedInvocList -= value; }
        }
        protected virtual void OnClosed(bool dialogResult)
        {
            EventHandler<CloseDialogEventArgs> handler = _closedInvocList;
            if (handler != null) handler(this, new CloseDialogEventArgs(dialogResult));
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

        #endregion
    }
}
