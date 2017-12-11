using System;
using System.Windows;

namespace InfrastructureLight.Wpf.Common.Factory
{
    using System.Reflection;

    public class ViewFactory : IViewFactory
    {
        public FrameworkElement CreateView<TViewModel>(TViewModel viewModel)
        {
            Type viewModelType = viewModel.GetType();

            string assemblyQualifiedName = viewModelType.AssemblyQualifiedName;
            assemblyQualifiedName = assemblyQualifiedName.Replace("ViewModel", "View");

            Type viewType = Type.GetType(assemblyQualifiedName);
            if (viewType == null)
                throw new ArgumentException(
                    string.Format("Unable to find view type: {0} for given view model.", assemblyQualifiedName), "viewModel");

            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            view.DataContext = viewModel;

            return view;
        }
    }
}
