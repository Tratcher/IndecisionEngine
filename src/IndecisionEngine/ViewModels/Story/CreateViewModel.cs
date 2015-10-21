using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IndecisionEngine.ViewModels.Story
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "Body")]
        public string Body { get; set; }
    }
}
