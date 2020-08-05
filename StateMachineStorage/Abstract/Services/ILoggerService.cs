using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Abstract.Services
{
    public interface ILoggerService
    {
        void Log(string caller, string message, string exceptionMessage = null);
    }
}
