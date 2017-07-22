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

    public abstract class EditViewModelBase<TEntity> : EditViewModelBase where TEntity : class
    {
        public void MapFrom(TEntity entity)
        {

        }
    }
}
