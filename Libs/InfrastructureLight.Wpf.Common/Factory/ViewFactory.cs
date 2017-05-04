using System;
using System.Windows;

namespace InfrastructureLight.Wpf.Common.Factory
{
    public class ViewFactory : IViewFactory
    {
        public FrameworkElement CreateView<TViewModel>(TViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();
            string typeName = viewModelType.FullName;
            typeName = typeName.Replace("ViewModel", "View");

            Type viewType = Type.GetType(typeName);
            if (viewType == null)
                throw new ArgumentException(
                    string.Format("Unable to find view type: {0} for given view model.", typeName), "viewModel");

            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            return view;
        }
    }
}
