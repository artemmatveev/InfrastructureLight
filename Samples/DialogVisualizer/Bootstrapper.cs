using Autofac;
using DialogVisualizer.Factory;
using InfrastructureLight.Wpf.Common.Factory;

namespace DialogVisualizer
{
    using ViewModels;

    public class Bootstrapper
    {
        static IContainer _container;
        internal static IContainer Container => _container;

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ViewFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ViewModelFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<InfrastructureLight.Wpf.Dialogs.DialogVisualizer>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<StaffUnitViewModel>();
            builder.RegisterType<UserViewModel>();

            _container = builder.Build();
        }
    }
}
