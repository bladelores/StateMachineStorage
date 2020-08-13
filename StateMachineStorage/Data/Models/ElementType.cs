using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class ElementType
    {
        public ElementType()
        {
            Element = new HashSet<Element>();
            SMImplementation = new HashSet<SMImplementation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Element> Element { get; set; }
        public virtual ICollection<SMImplementation> SMImplementation { get; set; }
    }
}
