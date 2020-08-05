using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class State
    {
        public State()
        {
            SMDefinition = new HashSet<SMDefinition>();
            SMImplementation = new HashSet<SMImplementation>();
            TransitionNewState = new HashSet<Transition>();
            TransitionOldState = new HashSet<Transition>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SMDefinitionId { get; set; }
        public string Name { get; set; }

        public virtual SMDefinition SMDefinitionNavigation { get; set; }
        public virtual ICollection<SMDefinition> SMDefinition { get; set; }
        public virtual ICollection<SMImplementation> SMImplementation { get; set; }
        public virtual ICollection<Transition> TransitionNewState { get; set; }
        public virtual ICollection<Transition> TransitionOldState { get; set; }
    }
}
