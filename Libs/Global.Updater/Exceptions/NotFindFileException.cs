namespace Global.Updater.Exceptions
{
    public class NotFindFileException : UpdaterException {
        public NotFindFileException(string path) : base($"Файлы на сервере не сущесвуют или не удалось найти путь: {path}") { }
    }
}
