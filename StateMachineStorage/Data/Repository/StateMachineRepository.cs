using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StateMachineStorage.Abstract.Repositories;
using StateMachineStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateMachineStorage.Data.Repository
{
    public class StateMachineRepository : IStateMachineRepository
    {
        private readonly StateMachineStorageContext _context;

        public StateMachineRepository(StateMachineStorageContext context)
        {
            _context = context;
        }

        public void AddStateMachine(StateMachineModel model)
        {
            var addedTriggers = new List<string>();
            var addedStates = new List<string>();
            
            var stateMachineDefinition = new SMDefinition
            {
                Name = model.Name,
                Agenda = model.Agenda,
                Version = model.Version,
                Definition = model.Definition.ToString(Formatting.None),
            };

            _context.SMDefinition.Add(stateMachineDefinition);
            _context.SaveChanges();
            
            var initialStateName = (string)model.Definition["initial"];
            var allObjects = model.Definition.DescendantsAndSelf().OfType<JObject>();

            var stateTokens = allObjects.Where(x => x.ContainsKey("kind"));
            var transitionTokens = allObjects.Where(x => x.ContainsKey("target"));

            foreach (var transition in transitionTokens)
            {
                if (!addedTriggers.Contains((string)transition["event"]))
                {
                    var newTrigger = new TransitionTrigger
                    {
                        Name = (string)transition["event"]
                    };

                    addedTriggers.Add(newTrigger.Name);

                    _context.TransitionTrigger.Add(newTrigger);
                }                
            }

            foreach (var state in stateTokens)
            {
                if (!addedStates.Contains((string)state["id"]))
                {
                    var newState = new State
                    {
                        SMDefinitionId = stateMachineDefinition.Id,
                        Name = (string)state["id"]
                    };

                    addedStates.Add(newState.Name);

                    _context.State.Add(newState);
                }     
            }

            _context.SaveChanges();

            foreach (var state in stateTokens)
            {
                var childTransitionTokens = state["states"] == null
                    ? state.DescendantsAndSelf().OfType<JObject>().Where(x => x.ContainsKey("target"))
                    : state["states"].Children().OfType<JObject>().Where(x => x.ContainsKey("target"));

                foreach (var childTransition in childTransitionTokens)
                {
                    var newTransition = new Transition
                    {
                        SMDefinitionId = stateMachineDefinition.Id,
                        OldStateId = _context.State.FirstOrDefault(x => x.Name.Equals((string)state["id"])).Id,
                        NewStateId = _context.State.FirstOrDefault(x => x.Name.Equals((string)childTransition["target"])).Id,
                        TransitionTriggerId = _context.TransitionTrigger.FirstOrDefault(x => x.Name.Equals((string)childTransition["event"])).Id
                    };

                    _context.Transition.Add(newTransition);
                }                
            }

            stateMachineDefinition.InitialStateId = _context.State.FirstOrDefault(x => x.Name.Equals(initialStateName)).Id;

            _context.SaveChanges();
        }

        public void AddStateMachineImplementation(StateImplementationModel model)
        {
            var stateMachineImplementation = new SMImplementation
            {
                SMDefinitionId = model.StateDefinitionId,
                Name = model.Name,
                Implemetation = model.Implementation.ToString(Formatting.None)
            };

            _context.SMImplementation.Add(stateMachineImplementation);
            _context.SaveChanges();
        }

        public SMDefinition GetStateMachineById(long id)
        {
            return _context.SMDefinition.FirstOrDefault(x => x.Id == id);
        }

        public List<SMDefinition> GetStateMachines(string agenda = null)
        {
            return agenda != null
                    ? _context.SMDefinition.Where(x => x.Agenda.Equals(agenda)).ToList()
                    : _context.SMDefinition.ToList();
        }

        public void UpdateStateMachine(StateMachineModel model)
        {
            var stateMachineToUpdate = _context.SMDefinition.FirstOrDefault(x => x.Name.Equals(model.Name) && x.Agenda.Equals(model.Agenda));
            
            stateMachineToUpdate.Version = model.Version;
            stateMachineToUpdate.Definition = model.Definition.ToString(Formatting.None);

            _context.SaveChanges();
        }

        public void UpdateStateMachineImplementation(StateImplementationModel model)
        {
            var stateMachineImplementationToUpdate = _context.SMImplementation.FirstOrDefault(x => x.SMDefinitionId == model.StateDefinitionId && x.Name.Equals(model.Name));

            stateMachineImplementationToUpdate.Implemetation = model.Implementation.ToString(Formatting.None);

            _context.SaveChanges();
        }
    }
}
