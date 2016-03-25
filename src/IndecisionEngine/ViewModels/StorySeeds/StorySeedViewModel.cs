using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StorySeeds
{
    public class StorySeedViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "First Entry")]
        public int? StoryEntryId { get; set; }

        [Display(Name = "First Entry")]
        public string FirstEntry { get; set; }

        public IEnumerable<StoryEntry> Entries { get; set; }
    }
}
