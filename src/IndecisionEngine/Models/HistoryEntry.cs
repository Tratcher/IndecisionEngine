using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndecisionEngine.Models
{
    public class HistoryEntry
    {
        public int Id { get; set; }
        public int ChoiceId { get; set; }
        public int EndEntryId { get; set; }
        public string EndState { get; set; }
    }
}
