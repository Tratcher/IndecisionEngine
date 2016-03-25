using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IndecisionEngine.Models
{
    public class StorySeed
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "First Entry")]
        public int? FirstEntryId { get; set; }
    }
}
