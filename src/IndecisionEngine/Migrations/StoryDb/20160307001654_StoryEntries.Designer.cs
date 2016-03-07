using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using IndecisionEngine.Models;

namespace IndecisionEngine.Migrations.StoryDb
{
    [DbContext(typeof(StoryDbContext))]
    [Migration("20160307001654_StoryEntries")]
    partial class StoryEntries
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

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
