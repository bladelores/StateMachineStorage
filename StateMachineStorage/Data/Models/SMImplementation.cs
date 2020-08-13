using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class SMImplementation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ElementId { get; set; }
        public long ElementTypeId { get; set; }
        public long SMDefinitionId { get; set; }
        public string Name { get; set; }
        public string Implemetation { get; set; }

        public virtual Element Element { get; set; }
        public virtual ElementType ElementType { get; set; }
        public virtual SMDefinition SMDefinition { get; set; }
    }
}
