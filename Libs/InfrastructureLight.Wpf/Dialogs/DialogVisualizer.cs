using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InfrastructureLight.Wpf.Common.Dialogs;
using System.Linq;

namespace InfrastructureLight.Wpf.Dialogs
{
    using Common.Factory;
    using ViewModels;

    public class DialogVisualizer : IDialogVisualizer
    {
        private readonly IDictionary<ViewModelBase, Window> _windows
            = new Dictionary<ViewModelBase, Window>();

        private readonly IViewFactory _viewFactory;
        public DialogVisualizer(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        #region Methods Show
        
        public void Show<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null)
            where T : ViewModelBase
        {
            Show<T, ViewModelBase>(viewModel, null, dialogSettings, callback);
        }
     
        public void Show<T, TOwner>(T viewModel, TOwner owner, IDialogSettings dialogSettings = null, Action<T> callback = null)
            where T : ViewModelBase where TOwner : ViewModelBase
        {
            var window = GetWindow(viewModel);
            if (window != null)
            {
                TryFocusedWindow(window);
            }
            else
            {
                window = RegisterViewModel(viewModel, dialogSettings, callback);
                if (window != null)
                {
                    if (owner != null)
                    {
                        window.Owner = GetWindow(owner);
                    }
                    
                    window.Show();
                }
            }
        }

        #endregion

        #region Methods ShowDialog
        
        public bool? ShowDialog<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null)
            where T : ViewModelBase
        {
            return ShowDialog<T, ViewModelBase>(viewModel, null, dialogSettings, callback);
        }
        
        public bool? ShowDialog<T, TOwner>(T viewModel, TOwner owner, IDialogSettings dialogSettings = null, Action<T> callback = null)
            where T : ViewModelBase where TOwner : ViewModelBase
        {
            if (GetWindow(viewModel) != null)
                throw new ArgumentException("Повторно вызвано открытие диалога ещё не закрытого диалогового окна", nameof(viewModel));

            var window = RegisterViewModel(viewModel, dialogSettings, callback);
            if (window != null)
            {
                var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);                
                window.Owner = owner != null
                    ? GetWindow(owner) ?? (activeWindow != null ? activeWindow : Application.Current.MainWindow)
                    : (activeWindow != null ? activeWindow : Application.Current.MainWindow);

                return window.ShowDialog();
            }
            return false;
        }
        
        public void Close<T>(T viewModel) where T : ViewModelBase
        {
            GetWindow(viewModel)?.Close();
        }

        #endregion

        #region Private

        /// <summary>
        ///     Возвращает объект <see cref="Window"/> который соответсвует ViewModel.
        ///     Если ViewModel соответвует UserControl, то он предварительно оборачивается <see cref="Window"/>        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private Window RegisterViewModel<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase
        {
            var view = _viewFactory.CreateView(viewModel);
            var viewType = view.GetType();

            Window window = null;
            if (viewType.IsSubclassOf(typeof(Window)))
            {
                window = view as Window;
            }
            else if (viewType.IsSubclassOf(typeof(UserControl)))
            {
                var uc = view as UserControl;
                window = new DialogWindow
                {
                    Content = uc,                    
                    SaveWindowPosition = true,
                    GlowBrush = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                if (!string.IsNullOrEmpty(viewModel.Title)) {
                    window.Title = viewModel.Title;
                }

                if (dialogSettings != null)
                {
                    if (!string.IsNullOrEmpty(dialogSettings.Title)) {
                        window.Title = dialogSettings.Title;
                    }
                    if (dialogSettings.DialogHeight != default(double)) {
                        window.Height = dialogSettings.DialogHeight;
                    }
                    if (dialogSettings.DialogWidth != default(double)) {
                        window.Width = dialogSettings.DialogWidth;
                    }
                }                
            }

            if (window != null)
            {
                viewModel.Saved += (s, e) => {                   
                    window.Close();                    
                    callback(viewModel);                    
                };

                viewModel.Canceled += (s, e) => {                                    
                    window.Close();
                };

                viewModel.Confirm += (s, e) => {
                    bool? result = MessageDialogHelper.ShowDialog("Подтверждение", e.Message,
                            MessageBoxButton.YesNo, messageBoxImage: MessageBoxImage.Question);

                    if (result == true) {
                        e.Callback();
                    }
                };

                viewModel.Failure += (s, e) => {
                    bool? result = MessageDialogHelper.ShowDialog("Ошибка", e.Message,
                            MessageBoxButton.OK, messageBoxImage: MessageBoxImage.Error);

                    if (result == true) {
                        e.Callback();
                    }
                };


                window.DataContext = viewModel;
                window.Closed += (s, e) => {
                    if (_windows.ContainsKey(viewModel))
                    {
                        _windows.Remove(viewModel);                        
                    }
                };

                _windows.Add(viewModel, window);
            }

            return window;
        }

        private Window GetWindow<T>(T viewModel) where T : ViewModelBase
        {
            return _windows.ContainsKey(viewModel)
                ? _windows[viewModel]
                : null;
        }

        private bool TryFocusedWindow(Window window)
        {
            var result = false;
            if (window != null && window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;                
            }            
            return result;
        }

        #endregion
    }
}
