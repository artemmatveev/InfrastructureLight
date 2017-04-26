using System.IO;

namespace InfrastructureLight.Common.Helpers
{
    public static class IOHelper
    {
        /// <summary>
        ///     Deletes the contents by the directory path
        /// </summary> 
        /// <param name="directoryPath"></param>       
        public static void DeleteDirectoryContents(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                var info = new DirectoryInfo(directoryPath);
                foreach (var file in info.GetFiles())
                {
                    file.Delete();
                }

                foreach (var dir in info.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        /// <summary>
        ///     Finds the folder by path. If the folder does not exist 
        ///     then creates the folder
        /// </summary> 
        /// <param name="directoryPath"></param>       
        public static void HasAndCreateFolder(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        ///     Exports the Resource by path
        /// </summary>   
        /// <param name="res"></param>
        /// <param name="pathFile"></param>     
        public static void ExportResource(byte[] res, string pathFile)
        {
            using (var fs = new FileStream(pathFile, FileMode.Create))
            {
                fs.Write(res, 0, res.Length);
                fs.Close();
            }
        }
    }
}
