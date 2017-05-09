using System;
using System.Linq;
using System.Windows.Threading;
using System.Collections.Generic;

using InfrastructureLight.Common.Extensions;
using InfrastructureLight.Common.Helpers;

namespace InfrastructureLight.Wpf.Behaviors
{
    using ViewModels;
    using Commands;

    public class SearchCommon<T> : ISearch where T : class
    {
        DispatcherTimer _filterTimer;
        List<T> _source;
        CatalogViewModelBase<T> _vm;
        DelegateCommand _searchCommand;

        public SearchCommon(CatalogViewModelBase<T> vm)
        {
            _vm = vm;
        }

        public DelegateCommand Searched()
        {
            return _searchCommand ??
                (_searchCommand = new DelegateCommand(action => Go(), action => CanGo())); ;
        }

        private void Go()
        {
            _source = _source ?? _vm.ItemsSource.ToList();

            if (_filterTimer == null)
            {
                _filterTimer = new DispatcherTimer();
                _filterTimer.Tick += (s, e2) =>
                {
                    _vm.ItemsSource = SearchHelper.Search(_source, _vm.SearchText).ToObservable();
                    _filterTimer.Stop();
                };
                _filterTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            }
            _filterTimer.Start();
        }

        private bool CanGo()
        {
            return true;
        }
    }
}
