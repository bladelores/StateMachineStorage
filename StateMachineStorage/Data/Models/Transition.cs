using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class Transition
    {
        public Transition()
        {
            SMImplementation = new HashSet<SMImplementation>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SMDefinitionId { get; set; }
        public long OldStateId { get; set; }
        public long NewStateId { get; set; }
        public long TransitionTriggerId { get; set; }

        public virtual State NewState { get; set; }
        public virtual State OldState { get; set; }
        public virtual SMDefinition SMDefinition { get; set; }
        public virtual TransitionTrigger TransitionTrigger { get; set; }
        public virtual ICollection<SMImplementation> SMImplementation { get; set; }
    }
}
