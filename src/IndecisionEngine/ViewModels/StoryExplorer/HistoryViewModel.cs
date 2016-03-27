using System.Collections.Generic;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StoryExplorer
{
    public class HistoryViewModel
    {
        public StorySeed Seed { get; set; }
        public IList<HistoryEntry> History { get; set; }
        public IEnumerable<StoryChoice> Choices { get; set; }
        public IEnumerable<StoryEntry> Entries { get; set; }
    }
}
