namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="System.Object"/>
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Indicates whether the specified object is null.
        /// </summary>
        public static bool IsNull(this object source)
            => source == null;        

        public static bool IsNotNull(this object source)
            => source != null;        
    }
}
