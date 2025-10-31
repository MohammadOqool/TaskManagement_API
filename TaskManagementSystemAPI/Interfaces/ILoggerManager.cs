using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystemAPI.Interfaces
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Error(string message);
        void Error(Exception e);
        void Warning(string message);
        void AddLine();
    }
}
