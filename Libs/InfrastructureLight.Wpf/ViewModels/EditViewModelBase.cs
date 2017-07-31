namespace InfrastructureLight.Wpf.ViewModels
{
    public abstract class EditViewModelBase : AsyncViewModel
    {

    }

    public abstract class EditViewModelBase<TEntity> : EditViewModelBase where TEntity : class
    {
        public void MapFrom(TEntity entity, params object[] parameters)
        {

        }
    }
}
