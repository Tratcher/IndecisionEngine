using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace IndecisionEngine.Models
{
    public class StoryEntry
    {
        public long Id { get; set; }
        public string Body { get; set; }
    }
}
