using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IndecisionEngine.Models
{
    public class StoryTransition
    {
        public int Id { get; set; }

        [Display(Name = "Prior Entry")]
        public int? PriorEntryId { get; set; }

        public string Preconditions { get; set; }

        [Display(Name = "Choice")]
        public int? ChoiceId { get; set; }

        public string Effects { get; set; }

        [Display(Name = "Next Entry")]
        public int? NextEntryId { get; set; }
    }
}
