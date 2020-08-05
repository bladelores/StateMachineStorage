using Newtonsoft.Json.Linq;

namespace StateMachineStorage.Models
{
    public class StateMachineModel
    {
        public string Agenda { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public JObject Definition { get; set; }
    }
}
