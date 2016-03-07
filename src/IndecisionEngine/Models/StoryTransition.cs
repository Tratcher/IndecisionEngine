using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndecisionEngine.Models
{
    public class StoryTransition
    {
        public int Id { get; set; }
        public int? PriorEntryId { get; set; }
        public string Preconditions { get; set; }
        public int? ChoiceId { get; set; }
        public string Effects { get; set; }
        public int? NextEntryId { get; set; }
    }
}
