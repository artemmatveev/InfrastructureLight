using System.IO;
using System.Xml;
using System.Reflection;

using NLog;
using NLog.Config;

namespace Global.NLog.Config
{
    public sealed class NLogConfig
    {
        public static void ConfigureNLog(string appName, string companyName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Global.NLog.Config.NLog.config"))
            {
                var xml = new StreamReader(stream).ReadToEnd()
                    .Replace("@companyName", companyName)
                    .Replace("@appName", appName);

                using (var sr = new StringReader(xml))
                {
                    using (XmlReader xr = XmlReader.Create(sr))
                    {
                        var config = new XmlLoggingConfiguration(xr, null);
                        LogManager.Configuration = config;
                    }
                }
            }
        }
    }
}
