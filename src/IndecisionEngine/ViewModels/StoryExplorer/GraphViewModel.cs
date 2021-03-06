﻿using System.Collections.Generic;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StoryExplorer
{
    public class GraphViewModel
    {
        public StorySeed Seed { get; set; }
        public IEnumerable<StoryEntry> Entries { get; set; }
        public IEnumerable<StoryTransition> Transitions { get; set; }
        public IEnumerable<StoryChoice> Choices { get; set; }
    }
}
