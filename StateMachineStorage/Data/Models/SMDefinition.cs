using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class SMDefinition
    {
        public SMDefinition()
        {
            SMImplementation = new HashSet<SMImplementation>();
            State = new HashSet<State>();
            Transition = new HashSet<Transition>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long InitialStateId { get; set; }
        public string Agenda { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Definition { get; set; }

        public virtual State InitialState { get; set; }
        public virtual ICollection<SMImplementation> SMImplementation { get; set; }
        public virtual ICollection<State> State { get; set; }
        public virtual ICollection<Transition> Transition { get; set; }
    }
}
