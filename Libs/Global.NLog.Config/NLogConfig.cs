using NLog;
using NLog.Config;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Global.NLog.Config
{
    using Properties;

    public static class NLogConfig
    {
        public static void ConfigureNLog(string appName, string companyName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(Resources.ConfigName))
            {
                var xml = new StreamReader(stream).ReadToEnd()
                    .Replace("@companyName", companyName).Replace("@appName", appName);

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
