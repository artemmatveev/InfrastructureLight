using InfrastructureLight.Common.Exceptions;
using InfrastructureLight.Common.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Global.Updater
{
    using Properties;

    public sealed class LocalNetworkUpdater : UpdaterBase
    {
        public LocalNetworkUpdater(string appName, string sourceDirectory, string companyName)
            : base(appName, sourceDirectory, companyName) { }

        protected override void DirectoryCopy(string sourceDirName, string targetDirName, bool copySubDirs)
        {
            try
            {
                var localDir = new DirectoryInfo(targetDirName);
                if (!localDir.Exists) { localDir = Directory.CreateDirectory(targetDirName); }

                var sourceDir = new DirectoryInfo(sourceDirName);
                if (!sourceDir.Exists)
                {
                    throw new NotFindFileException(sourceDirName);
                }
                else
                {
                    FileInfo[] files = sourceDir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        string targetPath = Path.Combine(targetDirName, file.Name);
                        if (File.Exists(targetPath)
                                && File.GetLastWriteTime(targetPath) == file.LastWriteTime
                                && new FileInfo(targetPath).Length == file.Length)
                            continue;

                        Trace.WriteLine($"{Resources.traceLoadPrefixMessage} {targetPath}", Resources.traceCategory);
                        file.CopyTo(targetPath, true);
                    }

                    // Удаление файлов в целевом каталоге, 
                    // которые отсутвуют на сервере:
                    FileInfo[] localFiles = localDir.GetFiles();
                    foreach (FileInfo localFile in localFiles.Where(localFile => files.All(info => info.Name != localFile.Name)))
                    {
                        Trace.WriteLine($"{Resources.traceDeletePrefixMessage} {localFile.FullName}", Resources.traceCategory);
                        localFile.Delete();
                    }

                    // Подкаталоги на сервере:
                    DirectoryInfo[] dirs = sourceDir.GetDirectories();

                    // Удаление подкаталогов в целевом каталоге, 
                    // которые отсутвуют на сервере:
                    DirectoryInfo[] localDirectories = localDir.GetDirectories();
                    foreach (DirectoryInfo localInfo in localDirectories.Where(localInfo => dirs.All(info => info.Name != localInfo.Name)))
                    {
                        Trace.WriteLine($"{Resources.traceDeletePrefixMessage} {localInfo.FullName}", Resources.traceCategory);
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
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.GetFullErrorInfo(), Resources.traceErrorCategoty);
            }
        }

    }
}
