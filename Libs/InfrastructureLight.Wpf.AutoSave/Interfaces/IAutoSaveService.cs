namespace InfrastructureLight.Wpf.AutoSave.Interfaces
{
    public interface IAutoSaveService
    {
        /// <summary>
        ///     Возвращает объект типа, реализующего интерфейс <see cref="IAutoRowSaver"/>, соответствующий заданному ключу.
        ///     Возвращает null, если объект ещё не создан, уже удалён, или если ключ не правильный.
        /// </summary>        
        IAutoRowSaver GetSaver(string key);
    }
}