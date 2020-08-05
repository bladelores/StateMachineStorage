using StateMachineStorage.Data;
using StateMachineStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Abstract.Repositories
{
    public interface IStateMachineRepository
    {
        void AddStateMachine(StateMachineModel model);
        void AddStateMachineImplementation(StateImplementationModel model);
        SMDefinition GetStateMachineById(long id);
        void UpdateStateMachine(StateMachineModel model);
        void UpdateStateMachineImplementation(StateImplementationModel model);
        List<SMDefinition> GetStateMachines(string agenda = null);
        //SMDefinition GetStateMachine(string agenda, string name, string version);
    }
}
