using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using IndecisionEngine.Models;

namespace IndecisionEngine.Migrations.StoryDb
{
    [DbContext(typeof(StoryDbContext))]
    [Migration("20160305191410_StoryTransitionMigration")]
    partial class StoryTransitionMigration
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

                    b.Property<long?>("NextEntryId");

                    b.Property<string>("Preconditions");

                    b.Property<long?>("PriorEntryId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("IndecisionEngine.Models.StoryTransition", b =>
                {
                    b.HasOne("IndecisionEngine.Models.StoryChoice")
                        .WithMany()
                        .HasForeignKey("ChoiceId");

                    b.HasOne("IndecisionEngine.Models.StoryEntry")
                        .WithMany()
                        .HasForeignKey("NextEntryId");

                    b.HasOne("IndecisionEngine.Models.StoryEntry")
                        .WithMany()
                        .HasForeignKey("PriorEntryId");
                });
        }
    }
}
