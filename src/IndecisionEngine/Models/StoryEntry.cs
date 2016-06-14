using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IndecisionEngine.Models
{
    public class StoryEntry
    {
        public int Id { get; set; }

        public string Body { get; set; }
    }
}
