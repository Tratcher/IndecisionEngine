using System.Collections.Generic;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StoryExplorer
{
    public class StoryExplorerViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
        public IEnumerable<StoryChoice> Choices { get; set; }
        public IEnumerable<StoryTransition> Transitions { get; set; }
    }
}
