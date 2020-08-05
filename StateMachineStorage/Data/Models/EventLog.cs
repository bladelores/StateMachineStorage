using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace StateMachineStorage.Data.Models
{
    public partial class EventLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CallerMethod { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime Date { get; set; }
    }
}
