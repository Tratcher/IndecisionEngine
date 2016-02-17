using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace IndecisionEngine.Models
{
    public class StoryDbContext : DbContext
    {
        public DbSet<StoryEntry> StoryEntries { get; set; }
    }
}