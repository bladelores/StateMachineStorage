using StateMachineStorage.Data.Models;

namespace StateMachineStorage.Abstract.Repositories
{
    public interface IEventLogRepository
    {
        void AddEventLog(EventLog item);
    }
}
