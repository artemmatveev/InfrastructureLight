using InfrastructureLight.Common.Extensions;
using InfrastructureLight.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace InfrastructureLight.Wpf.Behaviors
{
    using Commands;
    using ViewModels;

    public class SearchCommon<T> : ISearch where T : class
    {
        DispatcherTimer _filterTimer;
        List<T> _source;
        readonly CatalogViewModelBase<T> _vm;
        DelegateCommand _searchCommand;

        public SearchCommon(CatalogViewModelBase<T> vm)
        {
            _vm = vm;
        }

        public DelegateCommand Searched()
            => _searchCommand ??
                (_searchCommand = new DelegateCommand(action => Go(), action => true));

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
    }
}
