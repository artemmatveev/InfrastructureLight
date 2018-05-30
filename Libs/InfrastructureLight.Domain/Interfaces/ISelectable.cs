namespace InfrastructureLight.Domain.Interfaces
{
    /// <summary>
    ///     Describes the selectable item
    /// </summary>
    public interface ISelectable {
        /// <summary>
        ///     Return <see cref="bool.True" />,
        ///     if entity selected
        /// </summary>
        bool IsSelected { get; }
    }
}
