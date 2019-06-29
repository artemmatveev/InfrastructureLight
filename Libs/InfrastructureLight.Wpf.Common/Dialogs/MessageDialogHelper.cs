using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InfrastructureLight.Wpf.Common.Dialogs
{
    using Properties;

    public static class MessageDialogHelper
    {
        /// <summary>
        ///     The margins to wrap string content with.
        /// </summary>
        static Thickness StringContentMarginThickness = new Thickness(10, 10, 50, 10);

        /// <summary>
        ///     The margins to wrap non-string content with.
        /// </summary>
        static Thickness DefaultContentMarginThickness = new Thickness(5);

        /// <summary>
        ///     The minimum width for the content.
        /// </summary>
        static int ContentMinWidth = 250;

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
            MessageDialogWindow simpleDialogWindow = CreateMessageDialogWindow(title, resizeMode, messageBoxImage);
            SetMessageBoxButtons(simpleDialogWindow, messageBoxButton, null, null);

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
            MessageBoxImage messageBoxImage = MessageBoxImage.None, string OkContent = null, string CancelContent = null, bool? warningFlag = false)
        {
            return ConfigureWindow(title, content, resizeMode, messageBoxImage, messageBoxButton, OkContent, CancelContent, warningFlag).ShowDialog();
        }        

        /// <summary>
        ///     Показывает диалоговое окно и возвращает состояние флага(Больше не показывать)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="warningFlag"></param>
        /// <param name="messageBoxButton"></param>
        /// <param name="resizeMode"></param>
        /// <param name="messageBoxImage"></param>
        /// <returns></returns>
        public static (bool? Dialog, bool? Flag) ShowDialogAndWarningFlag(string title, object content, string OkContent, string CancelContent, bool? warningFlag,
            MessageBoxButton messageBoxButton = MessageBoxButton.OK, ResizeMode resizeMode = ResizeMode.NoResize,
            MessageBoxImage messageBoxImage = MessageBoxImage.None)
        {
            var simpleDialogWindow = ConfigureWindow(title, content, resizeMode, messageBoxImage, messageBoxButton, OkContent, CancelContent, warningFlag);

            return (simpleDialogWindow.ShowDialog(), simpleDialogWindow.xCheckBox.IsChecked);
        }

        #region Private members

        private static MessageDialogWindow ConfigureWindow(string title, object content, ResizeMode resizeMode, MessageBoxImage messageBoxImage,
                                                                                         MessageBoxButton messageBoxButton, string OkContent, string CancelContent, bool? warningFlag)
        {
            MessageDialogWindow simpleDialogWindow = CreateMessageDialogWindow(title, resizeMode, messageBoxImage, warningFlag);
            simpleDialogWindow.Content = WrapContent(content);
            simpleDialogWindow.SizeToContent = SizeToContent.WidthAndHeight;
            SetMessageBoxButtons(simpleDialogWindow, messageBoxButton, OkContent, CancelContent);

            return simpleDialogWindow;
        }        
        private static MessageDialogWindow CreateMessageDialogWindow(string title, ResizeMode resizeMode, MessageBoxImage messageBoxImage, bool? warningFlag = false)
        {
            var dialogWindow = new MessageDialogWindow
            {
                Title = title,
                ResizeMode = resizeMode,
                Image = messageBoxImage,
                GlowBrush = new SolidColorBrush(Color.FromArgb(0xcc, 0x1b, 0xa1, 0xe2))
            };

            if (Application.Current != null)
            {
                var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                dialogWindow.Owner = activeWindow ?? Application.Current.MainWindow;
                dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            dialogWindow.xCheckBox.Visibility = warningFlag == true ? Visibility.Visible : Visibility.Collapsed;

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
        private static void SetMessageBoxButtons(MessageDialogWindow window, MessageBoxButton messageBoxButton, string OkContent, string CancelContent)
        {
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    window.xButton1.Visibility = Visibility.Visible;
                    
                    window.xTextBlock1.Text = string.IsNullOrEmpty(OkContent) ? Resources.OKButtonContent : OkContent;
                    window.xImage1.Visibility = string.IsNullOrEmpty(OkContent) ? Visibility.Collapsed : Visibility.Visible;
                    window.xButton1.IsDefault = true;

                    window.xButton2.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.OKCancel:
                    window.xButton1.Visibility = Visibility.Visible;
                    window.xTextBlock1.Text = string.IsNullOrEmpty(OkContent) ? Resources.OKButtonContent : OkContent;
                    window.xImage1.Visibility = string.IsNullOrEmpty(OkContent) ? Visibility.Collapsed : Visibility.Visible;
                    window.xButton1.IsDefault = true;

                    window.xButton2.Visibility = Visibility.Visible;
                    window.xTextBlock2.Text = string.IsNullOrEmpty(CancelContent) ? Resources.CancelButtonContent : CancelContent;
                    window.xImage2.Visibility = string.IsNullOrEmpty(OkContent) ? Visibility.Collapsed : Visibility.Visible;
                    window.xButton2.IsCancel = true;
                    break;
                case MessageBoxButton.YesNoCancel:
                case MessageBoxButton.YesNo:

                    window.xButton1.Visibility = Visibility.Visible;
                    window.xTextBlock1.Text = string.IsNullOrEmpty(OkContent) ? Resources.YesButtonContent : OkContent;
                    window.xImage1.Visibility = string.IsNullOrEmpty(OkContent) ? Visibility.Collapsed : Visibility.Visible;
                    window.xButton1.IsDefault = true;
                                 
                    window.xButton2.Visibility = Visibility.Visible;
                    window.xTextBlock2.Text = string.IsNullOrEmpty(CancelContent) ? Resources.NoButtonContent : CancelContent;
                    window.xImage2.Visibility = string.IsNullOrEmpty(OkContent) ? Visibility.Collapsed : Visibility.Visible;
                    window.xButton2.IsCancel = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageBoxButton), messageBoxButton, null);
            }
        }        

        #endregion
    }
}
