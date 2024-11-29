using NLog;

namespace Usuarios.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
           Logger.Debug(message);
        }

        public void LogError(string message)
        {
            Logger.Error(message);
        }

        public void LogInfo(string message)
        {
            Logger.Info(message);
        }

        public void LogWarning(string message)
        {
            Logger.Warn(message);
        }
    }
}
