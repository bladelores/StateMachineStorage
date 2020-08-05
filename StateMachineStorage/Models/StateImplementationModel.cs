using Newtonsoft.Json.Linq;

namespace StateMachineStorage.Models
{
    public class StateImplementationModel
    {
        public long StateDefinitionId { get; set; }
        public string Name { get; set; }
        // public long TransitionId { get; set; }
        public JObject Implementation { get; set; }
    }
}
