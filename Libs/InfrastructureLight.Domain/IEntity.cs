namespace InfrastructureLight.Domain
{
    /// <summary>
    ///     Entity Interface
    /// </summary>
    public interface IEntity<T>
    {
        T Id { get; set; }        
    }
}
