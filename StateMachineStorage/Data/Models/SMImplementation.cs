using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class SMImplementation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SMDefinitionId { get; set; }
        public string Name { get; set; }
        public long StateId { get; set; }
        public long TransitionId { get; set; }
        public string Implemetation { get; set; }

        public virtual SMDefinition SMDefinition { get; set; }
        public virtual State State { get; set; }
        public virtual Transition Transition { get; set; }
    }
}
