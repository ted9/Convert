using Convert.Infrastruture.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Convert.Web.Infrastruture
{
    public class Log4NetLogger : ILogger
    {
        private ILog GetLogger(Type logginType)
        {
            return LogManager.GetLogger(logginType);
        }
        public void LogMessage(string msg)
        {
            WriteEntry(this.GetType(), LoggingLevel.Info, msg);
        }

        public void LogException(Exception exception)
        {
            WriteEntry(this.GetType(), LoggingLevel.Error, exception.ToString());
        }

        public void Log(Type logginType, LoggingLevel level, string format, object[] objectToLog)
        {
            WriteEntry(logginType, level, string.Format(format, objectToLog));
        }

        private void WriteEntry(Type logginType, LoggingLevel level, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine(message);
            sb.AppendLine();
            sb.AppendLine(BuildHttpContextData());
            var logger = GetLogger(logginType);
            if (level == LoggingLevel.Debug)
            {
                logger.Debug(message);
            }
            else if (level == LoggingLevel.Error)
            {
                logger.Error(message);
            }
            else if (level == LoggingLevel.Fatal)
            {
                logger.Fatal(message);
            }
            else if (level == LoggingLevel.Info)
            {
                logger.Info(message);
            }
            else if (level == LoggingLevel.Warn)
            {
                logger.Warn(message);
            }
        }

        private string BuildHttpContextData()
        {

            var sb = new StringBuilder();

            try
            {
                var ctx = HttpContext.Current;
                var request = ctx.Request;
                sb.AppendFormat("URL: {0}\n", request.Url.ToString());
                sb.AppendFormat("QueryString: {0}\n", request.QueryString);
                sb.AppendFormat("Is Secure: {0}\n", request.IsSecureConnection.ToString());
                sb.AppendFormat("User Agent: {0}\n", request.UserAgent);
                sb.AppendFormat("Authenticated: {0}\n", request.IsAuthenticated);
                sb.AppendFormat("Identity: {0}\n",
                    request.LogonUserIdentity == null ? string.Empty : request.LogonUserIdentity.Name);
                sb.AppendFormat("User: {0}\n",
                    (ctx.User == null || ctx.User.Identity == null) ? string.Empty : ctx.User.Identity.Name);

                sb.AppendFormat("Form Keys:\n{0}\n", BuildRequestForm(request));

                return sb.ToString();
            }
            catch (Exception)
            {
                return (sb.Length > 0) ? sb + " \nError collecting data\n" : "Error collecting data";
            }
        }

        private string BuildRequestForm(HttpRequest request)
        {
            try
            {
                
                var sb = new StringBuilder();

                foreach (string key in request.Form.Keys)
                {
                    if (key.Trim().ToLower() == "password")
                    {
                        sb.AppendFormat("Key Value: {0} {1}\n", key, "<restricted>");
                    }
                    else
                    {
                        sb.AppendFormat("Key Value: {0} {1}\n", key, request.Form[key]);
                    }
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
               
            }
        }
    }
}
