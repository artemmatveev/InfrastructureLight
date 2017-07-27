namespace InfrastructureLight.Wpf.ViewModels
{
    public abstract class EditViewModelBase : ViewModelBase
    {

    }

    public abstract class EditViewModelBase<TEntity> : EditViewModelBase where TEntity : class
    {
        public void MapFrom(TEntity entity, params object[] parameters)
        {

        }
    }
}
