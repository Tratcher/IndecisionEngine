using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;
using Microsoft.Data.Entity;

namespace IndecisionEngine.ViewModels.StoryExplorer
{
    public class StoryExplorerViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public IEnumerable<StoryChoice> Choices { get; set; }
        public IEnumerable<StoryTransition> Transitions { get; set; }
    }
}
