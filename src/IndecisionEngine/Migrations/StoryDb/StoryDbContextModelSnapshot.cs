using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using IndecisionEngine.Models;

namespace IndecisionEngine.Migrations.StoryDb
{
    [DbContext(typeof(StoryDbContext))]
    partial class StoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IndecisionEngine.Models.StoryChoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("IndecisionEngine.Models.StoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("IndecisionEngine.Models.StorySeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("StoryEntryId");

                    b.Property<string>("Title");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("IndecisionEngine.Models.StoryTransition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChoiceId");

                    b.Property<string>("Effects");

                    b.Property<int?>("NextEntryId");

                    b.Property<string>("Preconditions");

                    b.Property<int?>("PriorEntryId");

                    b.HasKey("Id");
                });
        }
    }
}
