using Microsoft.Extensions.Configuration;
using StateMachineStorage.Abstract.Repositories;
using StateMachineStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Data.Repository
{
    public class EventLogRepository: IEventLogRepository
    {
        public StateMachineStorageContext _context;

        public EventLogRepository(StateMachineStorageContext context)
        {
            _context = context;
        }

        public void AddEventLog(EventLog item)
        {
            _context.EventLog.Add(item);
            _context.SaveChanges();
        }
    }
}
