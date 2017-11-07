using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Global.Updater
{
    public sealed class Updater
    {
        private const string TraceCategory = "ПРОЦЕСС";
        
        private readonly string _targetDirectory;
        private readonly string _sourceDirectory;
        
        public Updater(string appName, string sourceDirectory)
        {
            _targetDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _targetDirectory = Path.Combine(_targetDirectory, "Diakont", "Updater", "_data", appName);

            _sourceDirectory = sourceDirectory;
        }

        #region Fields

        /// <summary>
        ///     Путь к директории назначения
        /// </summary>
        public string TargetDirectory
        {
            get { return _targetDirectory; }
        }

        /// <summary>
        ///     Путь к источнику файлов
        /// </summary>
        public string SourceDirectory
        {
            get { return _sourceDirectory; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Выполняет обновление программы
        /// </summary>
        public void Run()
        {
            Trace.WriteLine("Запуск обновления...", TraceCategory);
            Trace.WriteLine(string.Format("Целевая директория: {0}", _targetDirectory), TraceCategory);

            DirectoryCopy(_sourceDirectory, _targetDirectory, true);
            
            Trace.WriteLine("Обновление завершено", TraceCategory);
        }

        private static void DirectoryCopy(string sourceDirName, string targetDirName, bool copySubDirs)
        {            
            var sourceDir = new DirectoryInfo(sourceDirName);
            var localDir = new DirectoryInfo(targetDirName);

            if (!localDir.Exists)
            {
                localDir = Directory.CreateDirectory(targetDirName);
            }

            if (!sourceDir.Exists)
            {
                Trace.WriteLine("Файлы на сервере не сущесвуют или не удалось найти путь", "ОШИБКА");
            }
            else
            {
                try
                {
                    FileInfo[] files = sourceDir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        string tempPath = Path.Combine(targetDirName, file.Name);

                        if (File.Exists(tempPath) && File.GetLastWriteTime(tempPath) >= file.LastWriteTime)
                            continue;

                        Trace.WriteLine(string.Format("Загрузка {0}", tempPath), TraceCategory);
                        file.CopyTo(tempPath, true);
                    }

                    // Удаление файлов в целевом каталоге, 
                    // которые отсутвуют на сервере:
                    FileInfo[] localFiles = localDir.GetFiles();
                    foreach (FileInfo localFile in localFiles
                        .Where(localFile => files.All(info => info.Name != localFile.Name)))
                    {
                        Trace.WriteLine(string.Format("Удаление {0}", localFile.FullName), TraceCategory);
                        localFile.Delete();
                    }

                    // Подкаталоги на сервере:
                    DirectoryInfo[] dirs = sourceDir.GetDirectories();

                    // Удаление подкаталогов в целевом каталоге, 
                    // которые отсутвуют на сервере:
                    DirectoryInfo[] localDirectories = localDir.GetDirectories();
                    foreach (DirectoryInfo localInfo in
                        localDirectories.Where(localInfo => dirs.All(info => info.Name != localInfo.Name)))
                    {
                        Trace.WriteLine(string.Format("Удаление {0}", localInfo.FullName), TraceCategory);
                        localInfo.Delete(true);
                    }

                    // Если копируем подкаталоги:
                    if (copySubDirs)
                    {
                        foreach (DirectoryInfo subDir in dirs)
                        {
                            string tempPath = Path.Combine(targetDirName, subDir.Name);
                            DirectoryCopy(subDir.FullName, tempPath, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message, "ОШИБКА");
                }
            }
        }

        #endregion
    }
}
