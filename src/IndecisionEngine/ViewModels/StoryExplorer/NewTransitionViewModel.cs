using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StoryExplorer
{
    public class NewTransitionViewModel
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

        public IEnumerable<StoryEntry> Entries { get; set; }
        public IEnumerable<StoryChoice> Choices { get; set; }
    }
}
