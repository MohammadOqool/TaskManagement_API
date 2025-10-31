using NLog;
using TaskManagementSystemAPI.Interfaces;

namespace TaskManagementSystemAPI.Managers.Loggers
{
    public class LoggerHandler : ILoggerManager
    {
        private readonly IEnumerable<ILoggerManager> _loggers;

        public LoggerHandler(IEnumerable<ILoggerManager> loggers)
        {
            _loggers = loggers;
        }

        public void AddLine()
        {
            foreach (var logger in _loggers)
            {
                logger.AddLine();
            }
        }

        public void Error(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(message);
            }

        }

        public void Error(Exception e)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(e);
            }
        }

        public void Info(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Info(message);
            }
        }

        public void Warning(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Warning(message);
            }
        }
    }
}
