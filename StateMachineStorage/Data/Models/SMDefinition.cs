﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateMachineStorage.Data
{
    public partial class SMDefinition
    {
        public SMDefinition()
        {
            Element = new HashSet<Element>();
            SMImplementation = new HashSet<SMImplementation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? InitialStateId { get; set; }
        public string Agenda { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Definition { get; set; }

        public virtual ICollection<Element> Element { get; set; }
        public virtual ICollection<SMImplementation> SMImplementation { get; set; }
    }
}
