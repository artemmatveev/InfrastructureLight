using System.IO;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="System.IO.Stream"/>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        ///     Write a <see cref="System.IO.Stream"/> object 
        ///     to a byte[]
        /// </summary>
        public static byte[] ToArray(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[1024];

                int bytes;
                while ((bytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, bytes);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Save a <see cref="System.IO.Stream"/> object 
        ///     to write to the specified file
        /// </summary>
        public static void SaveStreamToFile(this Stream stream, string fileFullPath)
        {
            if (stream.Length == 0) return;

            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                byte[] bytesInStream = new byte[stream.Length];

                stream.Read(bytesInStream, 0, bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }
}
