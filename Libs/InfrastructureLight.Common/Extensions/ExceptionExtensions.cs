using System;

namespace InfrastructureLight.Common.Extensions
{
    /// <summary>
    ///     Extensions class for the <see cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {
        public static string GetFullErrorInfo(this Exception ex, string exMessage = "", int offset = 0)
        {
            if (ex != null)
            {
                string strOffset = new string(' ', offset);
                string indent1 = offset > 0 ? "|_" : "";
                string indent2 = offset > 0 ? "  " : "";

                exMessage += $"{strOffset}{indent1}{ex.Message}\r\n{strOffset}Type: {ex.GetType().Name}\r\n\r\n" +
                             $" {indent2}StackTrace:\r\n  {ex.StackTrace}\r\n";

                return ex.InnerException.GetFullErrorInfo(exMessage, offset + 1);
            }
            return exMessage;
        }
    }
}
