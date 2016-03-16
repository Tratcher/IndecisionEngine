using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using IndecisionEngine.Models;
using IndecisionEngine.Controllers;

namespace IndecisionEngine.Models
{
    public class StoryDbContext : DbContext
    {
        public DbSet<StoryEntry> StoryEntry { get; set; }
        public DbSet<StoryChoice> StoryChoice { get; set; }
        public DbSet<StoryTransition> StoryTransition { get; set; }
        public DbSet<StorySeed> StorySeed { get; set; }
    }
}