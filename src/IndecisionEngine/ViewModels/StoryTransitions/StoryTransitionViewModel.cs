using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StoryTransitions
{
    public class StoryTransitionViewModel
    {
        public StoryTransitionViewModel()
        {
        }

        public StoryTransitionViewModel(StoryTransition transition)
        {
            Id = transition.Id;
            PriorEntryId = transition.PriorEntryId;
            Preconditions = transition.Preconditions;
            ChoiceId = transition.ChoiceId;
            Effects = transition.Effects;
            NextEntryId = transition.NextEntryId;
        }

        public int Id { get; set; }

        [Display(Name = "Prior Entry")]
        public int? PriorEntryId { get; set; }

        [Display(Name = "Prior Entry")]
        public string PriorEntry { get; set; }

        public string Preconditions { get; set; }

        [Display(Name = "Choice")]
        public int? ChoiceId { get; set; }

        [Display(Name = "Choice")]
        public string Choice { get; set; }

        public string Effects { get; set; }

        [Display(Name = "Next Entry")]
        public int? NextEntryId { get; set; }

        [Display(Name = "Next Entry")]
        public string NextEntry { get; set; }

        public IEnumerable<StoryChoice> Choices { get; set; }

        public IEnumerable<StoryEntry> Entries { get; set; }
    }
}
