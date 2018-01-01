using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace InfrastructureLight.Wpf.Dialogs
{
    using Common.Factory;
    using ViewModels;

    public class ViewVisualizer : IViewVisualizer
    {
        private readonly IDictionary<ViewModelBase, Window> _windows
            = new Dictionary<ViewModelBase, Window>();

        private readonly IViewFactory _viewFactory;
        public ViewVisualizer(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        #region Methods Show

        /// <summary>
        ///     Отобразить окно для ViewModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        public void Show<T>(T viewModel)
            where T : ViewModelBase
        {
            Show<T, ViewModelBase>(viewModel, null);
        }

        /// <summary>
        ///     Отобразить окно для ViewModel
        ///     и задать владельца
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOwner"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        public void Show<T, TOwner>(T viewModel, TOwner owner)
            where T : ViewModelBase where TOwner : ViewModelBase
        {
            var window = GetWindow(viewModel);
            if (window != null)
            {
                TryFocusedWindow(window);
            }
            else
            {
                window = RegisterViewModel(viewModel);
                if (window != null)
                {
                    window.Owner = owner != null
                        ? GetWindow(owner) ?? Application.Current.MainWindow
                        : Application.Current.MainWindow;

                    window.Show();
                }
            }
        }

        #endregion

        #region Methods ShowDialog

        /// <summary>
        ///     Отобразить ДИАЛОГОВОЕ окно для ViewModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        public bool? ShowDialog<T>(T viewModel)
            where T : ViewModelBase
        {
            return ShowDialog<T, ViewModelBase>(viewModel, null);
        }

        /// <summary>
        ///     Отобразить ДИАЛОГОВОЕ окно для ViewModel
        ///     и задать владельца
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOwner"></typeparam>
        /// <param name="viewModel"></param>
        /// <param name="owner"></param>
        public bool? ShowDialog<T, TOwner>(T viewModel, TOwner owner)
            where T : ViewModelBase where TOwner : ViewModelBase
        {
            if (GetWindow(viewModel) != null)
                throw new ArgumentException("Повторно вызвано открытие диалога ещё не закрытого диалогового окна", nameof(viewModel));

            var window = RegisterViewModel(viewModel);
            if (window != null)
            {
                window.Owner = owner != null
                    ? GetWindow(owner) ?? Application.Current.MainWindow
                    : Application.Current.MainWindow;

                return window.ShowDialog();
            }
            return false;
        }

        #endregion

        #region Close

        /// <summary>
        ///     Закрыть открытое окно 
        ///     для ViewModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
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
        private Window RegisterViewModel<T>(T viewModel) where T : ViewModelBase
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
                window = new MetroWindow
                {
                    Content = uc,
                    Title = viewModel.Title,
                    SaveWindowPosition = true,
                    GlowBrush = Brushes.Black,
                    BorderThickness = new Thickness(0)
                };

                ResourceDictionary resource = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.Buttons.xaml")
                };

                window.Resources.MergedDictionaries.Add(resource);
            }

            if (window != null)
            {
                window.DataContext = viewModel;
                window.Closed += Window_Closed;

                _windows.Add(viewModel, window);
            }

            return window;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            var window = sender as Window;
            var viewModel = window?.DataContext as ViewModelBase;

            if (viewModel != null)
            {
                if (_windows.ContainsKey(viewModel))
                {
                    _windows.Remove(viewModel);
                }
            }
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
