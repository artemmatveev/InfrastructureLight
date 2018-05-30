using System;
using System.Collections.Generic;
using System.Windows.Input;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using InfrastructureLight.Wpf.Commands;

namespace InfrastructureLight.Wpf.AutoSave
{
    using Interfaces;

    public class GridControlSaveService : IAutoSaveService
    {
        private Dictionary<string, IAutoRowSaver> _savers 
            = new Dictionary<string, IAutoRowSaver>();

        private ICommand _attachGridCommand;
        private ICommand _detachGridCommand;

        public ICommand AttachGridCommand
        {
            get { return _attachGridCommand ?? (_attachGridCommand = new DelegateCommand<GridControl>(AttachGrid)); }
        }

        public void AttachGrid(GridControl grid)
        {
            string key = DXSerializer.GetSerializationID(grid);

            if (_savers.ContainsKey(key))
            {
                throw new InvalidOperationException("Попытка прикрепить GridControl, который уже был прикреплён");
            }

            GridControlAutoSaver saver = new GridControlAutoSaver(grid);

            _savers.Add(key, saver);
        }

        public ICommand DetachGridCommand
        {
            get { return _detachGridCommand ?? (_detachGridCommand = new DelegateCommand<GridControl>(DetachGrid)); }
        }

        public void DetachGrid(GridControl grid)
        {
            string key = DXSerializer.GetSerializationID(grid);

            if (!_savers.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    "Попытка открепить GridControl, который уже был откреплён или ещё не прикреплён");
            }

            _savers.Remove(key);
        }

        #region INewItemService
        
        public IAutoRowSaver GetSaver(string key)
        {
            if (!_savers.ContainsKey(key)) return null;

            return _savers[key];
        }

        #endregion
    }
}