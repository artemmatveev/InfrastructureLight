using System.Windows;

namespace InfrastructureLight.Wpf.Common.Factory
{
    public interface IViewFactory
    {
        FrameworkElement CreateView<TViewModel>(TViewModel viewModel);
    }
}
