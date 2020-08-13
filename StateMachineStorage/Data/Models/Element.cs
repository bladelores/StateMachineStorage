using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class Element
    {
        public Element()
        {
            SMImplementation = new HashSet<SMImplementation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ElementTypeId { get; set; }
        public long SMDefinitionId { get; set; }
        public long? OldStateId { get; set; }
        public long? NewStateId { get; set; }
        public long? TransitionTriggerId { get; set; }
        public string Name { get; set; }

        public virtual ElementType ElementType { get; set; }
        public virtual SMDefinition SMDefinition { get; set; }
        public virtual ICollection<SMImplementation> SMImplementation { get; set; }
    }
}
