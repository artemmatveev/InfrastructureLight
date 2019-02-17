using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InfrastructureLight.Wpf.ViewModels
{
    using Behaviors;
    using Commands;

    public abstract class CatalogViewModelBase : AsyncViewModel
    {
        protected ISearch _searchCommand;
        readonly ICommand _updateCommand;

        protected CatalogViewModelBase()
        {
            _updateCommand = new DelegateCommand(action => Update(), action => CanUpdate());
        }

        #region Fileds

        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; RaisePropertyChangedEvent(); }
        }

        #endregion

        #region Commands

        public ICommand SearchCommand => _searchCommand.Searched();
        public ICommand UpdateCommand => _updateCommand;
        protected virtual void Update()
        {
            RefreshAsynch();
        }
        protected virtual bool CanUpdate()
        {
            return true;
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
        protected CatalogViewModelBase()
        {
            _searchCommand = new SearchCommon<T>(this);
        }

        ObservableCollection<T> _itemsSource;
        public ObservableCollection<T> ItemsSource
        {
            get => _itemsSource ?? (_itemsSource = new ObservableCollection<T>());
            set { _itemsSource = value; RaisePropertyChangedEvent(); }
        }

        T _selectedItem;
        public virtual T SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; RaisePropertyChangedEvent(); }
        }

        ObservableCollection<T> _selectedItems;
        public virtual ObservableCollection<T> SelectedItems
        {
            get => _selectedItems ?? (_selectedItems = new ObservableCollection<T>());
            set { _selectedItems = value; RaisePropertyChangedEvent(); }
        }

        #region Commands

        protected override bool CanApply()
            => SelectedItem != null;

        #endregion
    }
}
