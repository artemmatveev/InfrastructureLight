using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace InfrastructureLight.Wpf.Common.Dialogs
{
    using Properties;

    public static class DialogHelper
    {
        /// <summary>
        ///     The margins to wrap string content with.
        /// </summary>
        public static Thickness StringContentMarginThickness = new Thickness(10, 10, 50, 10);

        /// <summary>
        ///     The margins to wrap non-string content with.
        /// </summary>
        public static Thickness DefaultContentMarginThickness = new Thickness(5);

        /// <summary>
        ///     The minimum width for the content.
        /// </summary>
        public static int ContentMinWidth = 250;

        /// <summary>
        ///     Shows a dialog with a title, a content, with a specified size and an optional scrollbar.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="content">Window content.</param>
        /// <param name="width">
        ///     Sets the width of the entire window.
        /// </param>
        /// <param name="height">
        ///     Sets the height of the entire window.
        /// </param>
        /// <param name="scrollable">Enables the scrollbar in the dialog window.</param>
        /// <param name="messageBoxButton">
        ///     Sets the buttons combination. <see cref="MessageBoxButton.YesNoCancel" /> is not
        ///     supported.
        /// </param>
        /// <param name="resizeMode">Resize mode.</param>
        /// <param name="messageBoxImage">An image to display in window.</param>
        /// <returns>Dialog result.</returns>
        public static bool? ShowDialog(string title, object content, double width, double height,
            bool scrollable = false, MessageBoxButton messageBoxButton = MessageBoxButton.OK,
            ResizeMode resizeMode = ResizeMode.NoResize, MessageBoxImage messageBoxImage = MessageBoxImage.None)
        {
            DialogWindow simpleDialogWindow = CreateDialogWindow(title, resizeMode, messageBoxImage);
            SetMessageBoxButtons(simpleDialogWindow, messageBoxButton);

            if (scrollable)
            {
                simpleDialogWindow.Content = new ScrollViewer
                {
                    Content = WrapContent(content)
                };
            }
            else
            {
                simpleDialogWindow.Content = WrapContent(content);
            }

            simpleDialogWindow.Width = simpleDialogWindow.MinWidth = width;
            simpleDialogWindow.Height = simpleDialogWindow.MinHeight = height;

            return simpleDialogWindow.ShowDialog();
        }

        /// <summary>
        ///     Shows a simple dialog with a title and a content, auto-sized by its width and height.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="content">Window content.</param>
        /// <param name="messageBoxButton">
        ///     Sets the buttons combination. <see cref="MessageBoxButton.YesNoCancel" /> is not
        ///     supported.
        /// </param>
        /// <param name="resizeMode">Resize mode.</param>
        /// <param name="messageBoxImage">An image to display in window.</param>
        /// <returns>Dialog result.</returns>
        public static bool? ShowDialog(string title, object content,
            MessageBoxButton messageBoxButton = MessageBoxButton.OK, ResizeMode resizeMode = ResizeMode.NoResize,
            MessageBoxImage messageBoxImage = MessageBoxImage.None)
        {
            DialogWindow simpleDialogWindow = CreateDialogWindow(title, resizeMode, messageBoxImage);
            simpleDialogWindow.Content = WrapContent(content);
            simpleDialogWindow.SizeToContent = SizeToContent.WidthAndHeight;

            SetMessageBoxButtons(simpleDialogWindow, messageBoxButton);

            return simpleDialogWindow.ShowDialog();
        }

        #region Private members

        private static DialogWindow CreateDialogWindow(string title, ResizeMode resizeMode, MessageBoxImage messageBoxImage)
        {
            var dialogWindow = new DialogWindow
            {
                Title = title,
                ResizeMode = resizeMode,
                Image = messageBoxImage,
                GlowBrush = new SolidColorBrush(Color.FromArgb(0xcc, 0x1b, 0xa1, 0xe2))
            };

            if (Application.Current != null)
            {
                dialogWindow.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            return dialogWindow;
        }

        private static object WrapContent(object content)
        {
            var s = content as string;
            if (s != null)
            {
                return new TextBlock
                {
                    Text = s,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = StringContentMarginThickness
                };
            }

            return new ContentControl
            {
                Margin = DefaultContentMarginThickness,
                Content = content,
                MinWidth = ContentMinWidth
            };
        }

        private static void SetMessageBoxButtons(DialogWindow window, MessageBoxButton messageBoxButton)
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    window.xButton1.Visibility = Visibility.Visible;
                    window.xButton1.Content = Resources.OKButtonContent;
                    window.xButton1.IsDefault = true;

                    window.xButton2.Visibility = Visibility.Collapsed;                    
                    break;
                case MessageBoxButton.OKCancel:
                    window.xButton1.Visibility = Visibility.Visible;
                    window.xButton1.Content = Resources.OKButtonContent;
                    window.xButton1.IsDefault = true;

                    window.xButton2.Visibility = Visibility.Visible;
                    window.xButton2.Content = Resources.CancelButtonContent;
                    window.xButton2.IsCancel = true;
                    break;
                case MessageBoxButton.YesNoCancel:
                case MessageBoxButton.YesNo:
                    window.xButton1.Visibility = Visibility.Visible;
                    window.xButton1.Content = Resources.YesButtonContent;
                    window.xButton1.IsDefault = true;

                    window.xButton2.Visibility = Visibility.Visible;
                    window.xButton2.Content = Resources.NoButtonContent;
                    window.xButton2.IsCancel = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("messageBoxButton", messageBoxButton, null);
            }
        }

        #endregion
    }
}
