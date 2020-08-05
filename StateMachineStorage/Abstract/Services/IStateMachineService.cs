using StateMachineStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Abstract.Services
{
    public interface IStateMachineService
    {
        bool ValidateStateMachine(StateMachineModel model);
    }
}
