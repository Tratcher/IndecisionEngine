using System.Collections.Generic;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StoryTransitions
{
    public class StoryTransitionsIndexViewModel
    {
        public IEnumerable<StoryTransition> Transitions { get; set; }

        public IEnumerable<StoryChoice> Choices { get; set; }

        public IEnumerable<StoryEntry> Entries { get; set; }
    }
}
