using InfrastructureLight.Wpf.Common.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InfrastructureLight.Wpf.Dialogs
{
    using Common.Factory;
    using InfrastructureLight.Wpf.Dialogs.Message;
    using ViewModels;

    public class DialogVisualizer : IDialogVisualizer
    {
        /// <summary>
        ///     The margins to wrap string content with.
        /// </summary>
        static Thickness StringContentMarginThickness = new Thickness(10, 10, 50, 10);

        private readonly IDictionary<ViewModelBase, Window> _windows
            = new Dictionary<ViewModelBase, Window>();

        private readonly IViewFactory _viewFactory;
        public DialogVisualizer(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        #region Methods Show

        public void Show<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase
                => Show<T, ViewModelBase>(viewModel, null, dialogSettings, callback);

        public void Show<T, TOwner>(T viewModel, TOwner owner, IDialogSettings dialogSettings = null, Action<T> callback = null)
            where T : ViewModelBase where TOwner : ViewModelBase
        {
            var window = GetWindow(viewModel);
            if (window != null)
            {
                FocusWindow(window);
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

        public bool? ShowDialog<T>(T viewModel, IDialogSettings dialogSettings = null, Action<T> callback = null) where T : ViewModelBase
            => ShowDialog<T, ViewModelBase>(viewModel, null, dialogSettings, callback);

        public bool? ShowDialog<T, TOwner>(T viewModel, TOwner owner, IDialogSettings dialogSettings = null, Action<T> callback = null)
            where T : ViewModelBase where TOwner : ViewModelBase
        {
            if (GetWindow(viewModel) != null)
                throw new ArgumentException("Повторно вызвано открытие диалога ещё не закрытого диалогового окна", nameof(viewModel));

            var window = RegisterViewModel(viewModel, dialogSettings, callback);
            if (window != null)
            {
                var activeWindow = Application.Current.Windows.OfType<Window>()
                    .SingleOrDefault(x => x.IsActive);

                if (owner != null) {
                    window.Owner = GetWindow(owner) ?? 
                        activeWindow ?? Application.Current.MainWindow;
                }
                else {
                    window.Owner = activeWindow ?? Application.Current.MainWindow;
                }
                
                return window.ShowDialog();
            }
            return false;
        }

        #endregion

        #region Metods ShowDialog for Message Dialog Style

        public bool? ShowDialog(string title, string content, ResizeMode resizeMode = ResizeMode.NoResize,
            MessageBoxImage messageBoxImage = MessageBoxImage.None, params DialogButton[] buttons)
        {
            //var vm = new MsgDialogViewModel();
            var window = new MsgDialogWindow
            {
                Title = title,
                ResizeMode = resizeMode,
                Image = messageBoxImage,
                SizeToContent = SizeToContent.WidthAndHeight,
                GlowBrush = new SolidColorBrush(Color.FromArgb(0xcc, 0x1b, 0xa1, 0xe2))
            };            

            window.Content = new TextBlock
            {
                Text = content,
                TextWrapping = TextWrapping.Wrap,
                Margin = StringContentMarginThickness
            };

            if (Application.Current != null)
            {
                var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                window.Owner = activeWindow ?? Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            // Buttons
            if (buttons != null)
            {
                foreach (var dialogButton in buttons)
                {
                    Button button = new Button();

                    // Content
                    if (dialogButton.Icon != null && ButtonIconSelector.Icons.ContainsKey(dialogButton.Icon.Value))
                    {
                        Image img = new Image { Source = ButtonIconSelector.Icons[dialogButton.Icon.Value] };
                        TextBlock tb = new TextBlock { Text = dialogButton.Content };
                        StackPanel sp = new StackPanel { Orientation = Orientation.Horizontal };
                        sp.Children.Add(img);
                        sp.Children.Add(tb);
                        button.Content = sp;
                    }
                    else
                    {
                        button.Content = dialogButton.Content;
                    }

                    button.IsDefault = dialogButton.IsDefault;
                    button.IsCancel = dialogButton.IsCancel;

                    // Closing
                    if (dialogButton.CloseWindow)
                    {
                        button.Click += window.Button_Click;
                    }

                    button.Command = dialogButton.Command;

                    window.xButtonPanel.Children.Add(button);
                }
            }

            return window.ShowDialog();
        }

        #endregion

        #region Closing

        public void Close<T>(T viewModel) where T : ViewModelBase
            => GetWindow(viewModel)?.Close();

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
                    SaveWindowPosition = false,
                    GlowBrush = Brushes.Black,
                    BorderThickness = new Thickness(0),
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                if (!string.IsNullOrEmpty(viewModel.Title))
                {
                    window.Title = viewModel.Title;
                }

                if (dialogSettings != null)
                {
                    if (!string.IsNullOrEmpty(dialogSettings.Title))
                    {
                        window.Title = dialogSettings.Title;
                    }
                    if (!string.IsNullOrEmpty(dialogSettings.Color))
                    {
                        (window as DialogWindow).ChangeAppStyle(dialogSettings.Color);
                    }

                    if (dialogSettings.DialogHeight != default(double))
                    {
                        window.Height = dialogSettings.DialogHeight;
                    }
                    if (dialogSettings.DialogWidth != default(double))
                    {
                        window.Width = dialogSettings.DialogWidth;
                    }

                    if (dialogSettings.DialogHeight == default(double) &&
                            dialogSettings.DialogWidth == default(double))
                    {
                        window.WindowState = WindowState.Maximized;
                    }
                }
            }

            if (window != null)
            {
                viewModel.Applied += (s, e) =>
                {
                    window.Close();
                    callback?.Invoke(viewModel);
                };

                viewModel.Canceled += (s, e) =>
                {
                    window.Close();
                };

                viewModel.Confirm += (s, e) =>
                {
                    bool? result = MessageDialogHelper.ShowDialog("Подтверждение", e.Message,
                            MessageBoxButton.YesNo, messageBoxImage: MessageBoxImage.Question);

                    if (result == true)
                    {
                        e.Callback();
                    }
                };

                viewModel.Failure += (s, e) =>
                {
                    bool? result = MessageDialogHelper.ShowDialog("Ошибка", e.Message,
                            MessageBoxButton.OK, messageBoxImage: MessageBoxImage.Error);

                    if (result == true)
                    {
                        e.Callback();
                    }
                };

                viewModel.Info += (s, e) =>
                {
                    bool? result = MessageDialogHelper.ShowDialog("Предупреждение", e.Message,
                            MessageBoxButton.OK, messageBoxImage: MessageBoxImage.Information);

                    if (result == true)
                    {
                        e.Callback();
                    }
                };

                window.DataContext = viewModel;

                window.Closing += (s, e) =>
                {
                    if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                    {
                        window.DialogResult = viewModel.DialogResult;
                    }
                };

                window.Closed += (s, e) =>
                {
                    if (_windows.ContainsKey(viewModel))
                    {
                        _windows.Remove(viewModel);
                    }

                    viewModel.OnClosed();
                };

                _windows.Add(viewModel, window);
            }

            return window;
        }

        private Window GetWindow<T>(T viewModel) where T : ViewModelBase
            => _windows.ContainsKey(viewModel)
                    ? _windows[viewModel]
                    : null;

        private void FocusWindow(Window window)
        {
            if (window != null)
            {
                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }

                window.Focus();
            }
        }

        #endregion
    }
}
