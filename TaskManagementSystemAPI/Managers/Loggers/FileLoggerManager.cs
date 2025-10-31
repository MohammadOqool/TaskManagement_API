using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using TaskManagementSystemAPI.Interfaces;

namespace TaskManagementSystemAPI.Managers.Loggers
{
    public class FileLoggerManager : ILoggerManager
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void Info(string message)
        {
            logger.Info(message);
        }
        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(Exception e)
        {
            logger.Error(e.Message.ToString());
            if (e.InnerException != null)
                logger.Error(e.InnerException);

            logger.Error(e);
        }
        public void Warning(string message)
        {
            logger.Warn(message);
        }
        public void AddLine()
        {
            logger.Info("--------------------------------------------------------------");
        }
    }
}
