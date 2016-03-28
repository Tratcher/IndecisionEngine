using System.ComponentModel.DataAnnotations;

namespace IndecisionEngine.Models
{
    public class HistoryEntry
    {
        public int Id { get; set; }
        public int ChoiceId { get; set; }
        public int EndEntryId { get; set; }

        [Display(Name = "State")]
        public string EndState { get; set; }
    }
}
