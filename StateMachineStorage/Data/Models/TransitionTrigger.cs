using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class TransitionTrigger
    {
        public TransitionTrigger()
        {
            Transition = new HashSet<Transition>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Transition> Transition { get; set; }
    }
}
