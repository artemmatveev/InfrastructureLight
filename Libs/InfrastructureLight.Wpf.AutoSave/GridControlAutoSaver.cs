using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;
using InfrastructureLight.Wpf.ViewModels;

namespace InfrastructureLight.Wpf.AutoSave
{
    using Interfaces;

    public class GridControlAutoSaver : IAutoRowSaver
    {
        // Элементы с ошибками валидации, 
        // котоые пытались сохранить:
        private List<ViewModelBase> _erroredItems 
            = new List<ViewModelBase>();

        // Пытамеся сохранить 
        // в данный момент:
        private bool _tryingToSaveErrored;

        public GridControlAutoSaver(GridControl grid)
        {
            Grid = grid;
            Window window = Window.GetWindow(grid);
            if (window != null) {
                window.Closing += (s, e) => OnWindowClosing(grid);
            }
        }

        #region Fields

        private GridControl Grid { get; }

        #endregion

        #region Methods
        
        public void AddItem<T>(T item, ICollection<T> itemsSource) where T : ViewModelBase
        {
            itemsSource.Add(item);
            Grid.CurrentItem = item;
            SetCurrentAsEdited();
        }

        public void SetCurrentAsEdited()
        {
            GridColumn firstEditableColumn = null;
            foreach (GridColumn column in Grid.Columns) {
                if (column.ReadOnly == false)
                {
                    firstEditableColumn = column;
                    break;
                }
            }

            if (firstEditableColumn == null) return;

            Grid.CurrentColumn = firstEditableColumn;
            Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;

            Grid.Dispatcher.BeginInvoke(new Action(() =>
            {
                Grid.View.ShowEditor();
                currentDispatcher.BeginInvoke(new Action(() =>
                {
                    var t = Grid.View.ActiveEditor.EditValue;
                    Grid.View.ActiveEditor.EditValue = new object();
                    Grid.View.ActiveEditor.EditValue = t;
                }), DispatcherPriority.Render);

            }), DispatcherPriority.Render);
        }

        public bool TrySave<T>(T item, Action<T> saveToRepositoryMethod) where T : ViewModelBase
        {
            if (item == null) return false;

            // Если ошибка, добавляем в список ошибочных, 
            // чтобы потом сохранить их:
            if (item.HasErrors || GridHasErrors())
            {
                if (!_erroredItems.Contains(item))
                    _erroredItems.Add(item);

                return false;
            }

            saveToRepositoryMethod(item);

            // Сохраняем те, ктороые 
            // были с ошибками:
            SaveErrored(item, saveToRepositoryMethod);

            return true;
        }

        public void Remove<T>(T item, ICollection<T> itemsSource, Action<T> saveToRepositoryMethod) where T : ViewModelBase, IRaisingPropertyChanged
        {
            itemsSource.Remove(item);

            // Сохраняем те, ктороые были 
            // с ошибками:
            SaveErrored(item, saveToRepositoryMethod);

            foreach (T it in itemsSource) {
                it.RaiseAllPropertiesChanged();
            }
        }

        public bool GridHasErrors()
        {
            return Grid.View.HasValidationError;
        }

        /// <summary>
        ///     Сохранение элементов, которые были с ошибками. Удаляем из этого списка текущий
        /// </summary>
        /// <param name="current">Текущий элемент. Его удаляем из списка ошибочных</param>
        private void SaveErrored<T>(T current, Action<T> saveToRepositoryMethod) where T : ViewModelBase
        {
            if (!_tryingToSaveErrored)
            {
                if (_erroredItems.Contains(current))
                {
                    _erroredItems.Remove(current);
                }

                _tryingToSaveErrored = true;
                for (var i = 0; i < _erroredItems.Count; i++)
                {
                    var erroredItem = _erroredItems[i];

                    if (TrySave((T)erroredItem, saveToRepositoryMethod))
                    {
                        _erroredItems.Remove(erroredItem);
                        i--;
                    }

                }
                _tryingToSaveErrored = false;
            }
        }

        private void OnWindowClosing(GridControl grid)
        {
            ViewModelBase vm = grid.DataContext as ViewModelBase;

            if (grid.View.IsEditing)
            {
                grid.View.PostEditor();
                vm?.CancelCommand.Execute(null);
            }
        }

        #endregion
    }
}