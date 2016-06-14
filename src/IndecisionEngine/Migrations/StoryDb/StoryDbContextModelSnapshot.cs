using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using IndecisionEngine.Models;

namespace IndecisionEngine.Migrations.StoryDb
{
    [DbContext(typeof(StoryDbContext))]
    partial class StoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IndecisionEngine.Models.StoryChoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.HasKey("Id");

                    b.ToTable("StoryChoice");
                });

            modelBuilder.Entity("IndecisionEngine.Models.StoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.HasKey("Id");

                    b.ToTable("StoryEntry");
                });

            modelBuilder.Entity("IndecisionEngine.Models.StorySeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FirstEntryId");

                    b.Property<string>("InitialState");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("StorySeed");
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

                    b.ToTable("StoryTransition");
                });
        }
    }
}
