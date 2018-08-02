using System;
using NLog;
using System.Runtime.CompilerServices;

namespace InfrastructureLight.Common.Extensions
{
    public struct LogBody
    {
        /// <summary>
        ///     Пользователь
        /// </summary>
        public string CurrentUser { get; set; }
        /// <summary>
        ///     Сессия
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        ///     Токен
        /// </summary>
        public string TokenId { get; set; }
        /// <summary>
        ///     Название БД
        /// </summary>
        public string DBName { get; set; }
        /// <summary>
        ///     Название организации
        /// </summary>
        public string OrgName { get; set; }
        /// <summary>
        ///     Контроллер или ViewModel
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        ///     Action или Method
        /// </summary>
        public string Action { get; set; }        
        public string EncryptedRoute { get; set; }
        /// <summary>
        ///     Маршрут
        /// </summary>
        public string Route => $"{Controller}/{Action}";
        /// <summary>
        ///     Истоник ошибки (Exception.Source)
        /// </summary>
        public string Source { get; set; }        
        public string SQLCommand { get; set; }
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Имя метода или свойства
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        ///     Полный путь исходного файла, содержащего вызывающий объект. Это путь к файлу во время компиляции
        /// </summary>
        public string SourceFilePath { get; set; }
        /// <summary>
        ///     Номер строки в исходном файле, в которой вызывается метод
        /// </summary>
        public int SourceLineNumber { get; set; }

        const string None = "[None]";
        public override string ToString()
          =>
            $"MemberName: {MemberName}\r\n" +
            $"SourceFilePath: {SourceFilePath}\r\n" +
            $"SourceLineNumber: {SourceLineNumber}\r\n" +
            $"User: {CurrentUser}\r\n" +
            $"SessionId: {SessionId}\r\n" +
            $"TokenId: {TokenId ?? None}\r\n" +
            $"Encrypted Route: {EncryptedRoute}\r\n" +
            $"Route: {Route}\r\n" +
            $"Source: {Source}\r\n" +
            $"DBName: {DBName}\r\n" +            
            $"SQL Command: {SQLCommand ?? None}\r\n" +
            $"Exception message:\r\n  {ErrorMessage}\r\n";            
    }

    /// <summary>
    ///     Extensions class for the <see cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {        
        const string SqlCommand = nameof(SqlCommand);

        /// <summary>
        ///     Возвращает Sql запрос 
        ///     из ошибки:
        /// </summary>        
        public static string GetSqlCommand(this Exception exception) 
            => (string)exception.Data[SqlCommand];

        /// <summary>
        ///     Пишет Sql запрос
        ///     в ошибку:
        /// </summary>        
        public static void SetSqlCommand(this Exception exception, string sql)
            => exception.Data[SqlCommand] = sql;
        

        /// <summary>
        ///     Возвращает дату в формате
        ///     день.месяц.год час_минута_секунда_милисекунда:
        /// </summary>
        /// <returns></returns>
        static string GetLogDate() 
            => $"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.{DateTime.Now.Millisecond}";
                
        /// <summary>
        ///     Возвращает полную информацию 
        ///     об ошибке:
        /// </summary>        
        public static string GetFullErrorInfo(this Exception ex, string exMessage = "", int offset = 0)
        {
            if (ex != null) {
                string strOffset = new string(' ', offset);
                string indent1 = offset > 0 ? "|_" : "";
                string indent2 = offset > 0 ? "  " : "";

                exMessage += $"{strOffset}{indent1}{ex.Message}\r\n{strOffset}Type: {ex.GetType().Name}\r\n\r\n" +
                             $" {indent2}StackTrace:\r\n  {ex.StackTrace}\r\n";

                return ex.InnerException.GetFullErrorInfo(exMessage, offset + 1);
            }
            return exMessage;
        }

        /// <summary>
        ///     Отправка сообщения в лог:
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void ToLog(this string errorMessage) 
            => LogManager.GetCurrentClassLogger().Error(errorMessage);        

        /// <summary>
        ///     Отправка сообщения в лог:
        /// </summary>        
        public static void ToLog(this string errorMessage, LogBody logBody,
                    [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", 
                    [CallerLineNumber] int sourceLineNumber = 0)
        {
            logBody.ErrorMessage = errorMessage;
            logBody.MemberName = memberName;
            logBody.SourceFilePath = sourceFilePath;
            logBody.SourceLineNumber = sourceLineNumber;

            LogManager.GetCurrentClassLogger().Error(logBody.ToString());
        }
    }
}
