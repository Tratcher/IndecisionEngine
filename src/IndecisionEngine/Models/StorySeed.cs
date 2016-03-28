using System.ComponentModel.DataAnnotations;

namespace IndecisionEngine.Models
{
    public class StorySeed
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "First Entry")]
        public int? FirstEntryId { get; set; }

        [Display(Name = "Initial State")]
        public string InitialState { get; set; }
    }
}
