using System;
using System.Windows;
using System.Windows.Controls;

namespace InfrastructureLight.Wpf.Common.Dialogs
{
    internal class ImageSelector : DataTemplateSelector
    {
        public DataTemplate None { get; set; }
        public DataTemplate Error { get; set; }
        public DataTemplate Question { get; set; }
        public DataTemplate Warning { get; set; }
        public DataTemplate Information { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null) return null;

            switch ((MessageBoxImage)item)
            {
                case MessageBoxImage.None:
                    return None;
                case MessageBoxImage.Error:
                    return Error;
                case MessageBoxImage.Question:
                    return Question;
                case MessageBoxImage.Warning:
                    return Warning;
                case MessageBoxImage.Information:
                    return Information;
                default:
                    throw new ArgumentOutOfRangeException("item", item, null);
            }
        }
    }
}
