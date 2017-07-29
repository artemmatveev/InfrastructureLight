using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InfrastructureLight.Wpf.ViewModels
{
    using Behaviors;
    using Commands;
    using EventArgs;

    public abstract class CatalogViewModelBase : AsyncViewModel
    {
        #region Fileds
       
        string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChangedEvent(); }
        }

        #endregion

        #region Commands

        protected ISearch _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand.Searched();
            }
        }

        private ICommand _updateCommand;
        public ICommand UpdateCommand
        {
            get
            {
                return _updateCommand ??
                    (_updateCommand = new DelegateCommand(action => Update(), action => CanUpdate()));
            }
        }
        protected virtual void Update()
        {
            RefreshAsynch();
        }
        protected virtual bool CanUpdate()
        {
            return true;
        }

        #endregion

        #region Events
       
        private EventHandler<OpenDialogEventArgs<ViewModelBase>> _editInvocList;
        public event EventHandler<OpenDialogEventArgs<ViewModelBase>> EditDialog
        {
            add
            {
                if (_editInvocList == null || _editInvocList.GetInvocationList()
                    .All(m => m.Method != value.Method))
                {
                    _editInvocList += value;
                }
            }
            remove { _editInvocList -= value; }
        }
        protected virtual void OnEditDialog(ViewModelBase viewModel, Action<ViewModelBase> callback = null)
        {
            EventHandler<OpenDialogEventArgs<ViewModelBase>> handler = _editInvocList;
            if (handler != null)
            {
                handler(this, new OpenDialogEventArgs<ViewModelBase>(viewModel, callback));
            }
        }

        #endregion

        public virtual void RefreshAsynch()
        {
            Go(Refresh);
        }

        public virtual void Refresh()
        {

        }
    }

    public abstract class CatalogViewModelBase<T> : CatalogViewModelBase where T : class
    {
        public CatalogViewModelBase()
        {
            _searchCommand = new SearchCommon<T>(this);
        }
       
        ObservableCollection<T> _itemsSource;
        public ObservableCollection<T> ItemsSource
        {
            get { return _itemsSource ?? (_itemsSource = new ObservableCollection<T>()); }
            set { _itemsSource = value; RaisePropertyChangedEvent(); }
        }
        
        T _selectedItem;
        public T SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChangedEvent(); }
        }
        
    }
}
