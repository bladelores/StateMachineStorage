﻿using Newtonsoft.Json.Linq;

namespace StateMachineStorage.Models
{
    public class StateImplementationModel
    {
        public long StateDefinitionId { get; set; }
        public long ElementTypeId { get; set; }
        public long ElementId { get; set; }
        public string Name { get; set; }        
        public JObject Implementation { get; set; }
    }
}
