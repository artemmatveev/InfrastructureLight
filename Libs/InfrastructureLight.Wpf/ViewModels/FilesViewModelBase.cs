using System.Windows.Input;

namespace InfrastructureLight.Wpf.ViewModels
{
    using Commands;

    public abstract class FilesViewModelBase<T> : CatalogViewModelBase<T> where T : class
    {
        readonly ICommand _addFileCommand;
        readonly ICommand _deleteFileCommand;
        readonly ICommand _openFileCommand;

        protected FilesViewModelBase()
        {
            _addFileCommand = new DelegateCommand(action => AddFile(), action => CanAddFile());
            _deleteFileCommand = new DelegateCommand(action => DeleteFile(), action => CanDeleteFile());
            _openFileCommand = new DelegateCommand(action => OpenFile(), action => CanOpenFile());
        }

        #region Commands

        public ICommand AddFileCommand => _addFileCommand;
        protected virtual void AddFile() => RefreshAsynch();
        protected virtual bool CanAddFile() => true;

        public ICommand DeleteFileCommand => _deleteFileCommand;
        protected virtual void DeleteFile() => RefreshAsynch();
        protected virtual bool CanDeleteFile() => true;

        public ICommand OpenFileCommand => _openFileCommand;
        protected virtual void OpenFile() => RefreshAsynch();
        protected virtual bool CanOpenFile() => true;

        #endregion
    }
}
