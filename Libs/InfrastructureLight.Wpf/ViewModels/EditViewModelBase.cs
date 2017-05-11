using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLight.Wpf.ViewModels
{
    public abstract class EditViewModelBase : ViewModelBase
    {

    }

    public abstract class EditViewModelBase<T> : EditViewModelBase where T : class
    {
        public void MapFrom(T entity)
        {

        }
    }
}
