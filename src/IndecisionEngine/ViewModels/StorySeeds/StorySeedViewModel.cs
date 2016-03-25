using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IndecisionEngine.Models;

namespace IndecisionEngine.ViewModels.StorySeeds
{
    public class StorySeedViewModel
    {
        public StorySeedViewModel()
        {
        }

        public StorySeedViewModel(StorySeed storySeed)
        {
            Id = storySeed.Id;
            Title = storySeed.Title;
            FirstEntryId = storySeed.FirstEntryId;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "First Entry")]
        public int? FirstEntryId { get; set; }

        [Display(Name = "First Entry")]
        public string FirstEntry { get; set; }

        public IEnumerable<StoryEntry> Entries { get; set; }
    }
}
