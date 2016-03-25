using System.Collections.Generic;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StorySeeds
{
    public class StorySeedsIndexViewModel
    {
        public IEnumerable<StorySeed> Seeds { get; set; }

        public IEnumerable<StoryEntry> Entries { get; set; }
    }
}
