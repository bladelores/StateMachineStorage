using StateMachineStorage.Abstract.Services;
using StateMachineStorage.Models;
using System;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Services
{
    public class StateMachineService : IStateMachineService
    {
        private readonly ILoggerService _loggerService;

        public StateMachineService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public bool ValidateStateMachine(StateMachineModel model)
        {
            string myschemaJson = @"{
                'description': 'An employee', 'type': 'object',
                'properties':
                {
                   'name': {'type':'string'},
                   'id': {'type':'string'},
                   'company': {'type':'string'},
                   'role': {'type':'string'},
                   'skill': {'type': 'array',
                   'items': {'type':'string'}
                }
            }";

            JsonSchema schema = JsonSchema.Parse(myschemaJson);

            return model.Definition.IsValid(schema);
        }
    }
}
