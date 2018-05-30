using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentFTP;
using InfrastructureLight.Common.Extensions;

namespace Global.Updater
{
    using Properties;
    using Exceptions;

    public sealed class FTPNetworkUpdater : UpdaterBase
    {        
        private readonly string _host, _login, _password;
        private readonly int _port;        

        public FTPNetworkUpdater(string appName, string sourceDirectory, string companyName, string host, int port, string login, string password)
            : base(appName, sourceDirectory, companyName) {
                _host = host;
                _port = port;
                _login = login;
                _password = password;
        }

        protected override void DirectoryCopy(string sourceDirName, string targetDirName, bool copySubDirs)
        {
            using (FtpClient client = new FtpClient(_host, _port, _login, _password)) {                             
                try {
                    client.Connect();                    
                    client.RetryAttempts = 3; 

                    var localDir = new DirectoryInfo(targetDirName);
                    if (!localDir.Exists) { localDir = Directory.CreateDirectory(targetDirName); }

                    if (!client.FileExists(sourceDirName)) {
                        throw new NotFindFileException(sourceDirName);
                    }
                    else {
                        var items = client.GetListing(sourceDirName);
                        var files = items.Where(i => i.Type == FtpFileSystemObjectType.File);
                        var directories = items.Where(i => i.Type == FtpFileSystemObjectType.Directory);

                        foreach (FtpListItem file in files)
                        {
                            string targetPath = Path.Combine(targetDirName, file.Name);                                                       
                            if (File.Exists(targetPath)
                                    && File.GetLastWriteTime(targetPath) >= client.GetModifiedTime(file.FullName)
                                    && new FileInfo(targetPath).Length == client.GetFileSize(file.FullName))
                                    //&& new FileInfo(targetPath).GetHashCode() == client.GetHash(file.FullName).GetHashCode())
                                continue;

                            Trace.WriteLine($"{Resources.traceLoadPrefixMessage} {targetPath}", Resources.traceCategory);
                            client.DownloadFile(sourceDirName + '/' + file.Name, targetPath, false);
                        }

                        // Удаление файлов в целевом каталоге, 
                        // которые отсутвуют на сервере:
                        var localFiles = localDir.GetFiles();
                        foreach (FileInfo localFile in localFiles.Where(localFile => files.All(info => info.Name != localFile.Name))) {
                            Trace.WriteLine($"{Resources.traceDeletePrefixMessage} {localFile.FullName}", Resources.traceCategory);
                            localFile.Delete();
                        }

                        // Удаление подкаталогов в целевом каталоге, 
                        // которые отсутвуют на сервере:
                        DirectoryInfo[] localDirectories = localDir.GetDirectories();
                        foreach (DirectoryInfo localDirectory in localDirectories.Where(localInfo => directories.All(info => info.Name != localInfo.Name))) {
                            Trace.WriteLine($"{Resources.traceDeletePrefixMessage} {localDirectory.FullName}", Resources.traceCategory);
                            localDirectory.Delete(true);
                        }

                        // Если копируем подкаталоги:
                        if (copySubDirs) {
                            foreach (FtpListItem directory in directories) {
                                string tempPath = Path.Combine(targetDirName, directory.Name);
                                DirectoryCopy(Path.Combine(sourceDirName, directory.FullName), tempPath, true);
                            }
                        }
                    }

                    client.Disconnect();
                }
                catch (Exception ex) {
                    Trace.WriteLine(ex.GetFullErrorInfo(), Resources.traceErrorCategoty);
                }                
            }
        }
    }
}
