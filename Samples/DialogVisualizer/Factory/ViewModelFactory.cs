using System;
using System.Collections.Generic;

using Autofac;
using InfrastructureLight.Wpf.Factory;
using InfrastructureLight.Wpf.ViewModels;

namespace DialogVisualizer.Factory
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly object _locked = new object();

        private readonly IDictionary<Type, ViewModelBase> _dictionary
            = new Dictionary<Type, ViewModelBase>();

        public T Get<T>() where T : ViewModelBase
        {
            lock (_locked)
            {
                if (!_dictionary.ContainsKey(typeof(T)))
                {
                    _dictionary[typeof(T)] = Bootstrapper.Container.Resolve<T>();
                }

                return (T)_dictionary[typeof(T)];
            }
        }

        public T Create<T>() where T : ViewModelBase
        {
            lock (_locked)
            {
                return Bootstrapper.Container.Resolve<T>();
            }
        }
    }
}
