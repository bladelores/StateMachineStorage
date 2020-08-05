using StateMachineStorage.Abstract.Repositories;
using StateMachineStorage.Abstract.Services;
using StateMachineStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IEventLogRepository _logRepository;

        public LoggerService(IEventLogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void Log(string caller, string message, string exceptionMessage = null)
        {
            _logRepository.AddEventLog(new EventLog
            {
                CallerMethod = caller,
                Message = message.Substring(0, message.Length > 2000 ? 2000 : message.Length),
                Exception = exceptionMessage,
                Date = DateTime.UtcNow
            });
        }
    }
}
