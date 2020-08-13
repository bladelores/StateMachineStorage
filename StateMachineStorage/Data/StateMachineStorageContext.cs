using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StateMachineStorage.Data.Models;

namespace StateMachineStorage.Data
{
    public partial class StateMachineStorageContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public StateMachineStorageContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public StateMachineStorageContext(DbContextOptions<StateMachineStorageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Element> Element { get; set; }
        public virtual DbSet<ElementType> ElementType { get; set; }
        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<SMDefinition> SMDefinition { get; set; }
        public virtual DbSet<SMImplementation> SMImplementation { get; set; }
        public virtual DbSet<TransitionTrigger> TransitionTrigger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Element>(entity =>
            {
                entity.ToTable("Element", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SMDefinitionId).HasColumnName("SMDefinitionId");

                entity.HasOne(d => d.ElementType)
                    .WithMany(p => p.Element)
                    .HasForeignKey(d => d.ElementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Element_ElementType");

                entity.HasOne(d => d.SMDefinition)
                    .WithMany(p => p.Element)
                    .HasForeignKey(d => d.SMDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Element_SMDefinition");
            });

            modelBuilder.Entity<ElementType>(entity =>
            {
                entity.ToTable("ElementType", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.ToTable("EventLog", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CallerMethod).HasMaxLength(200);

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<SMDefinition>(entity =>
            {
                entity.ToTable("SMDefinition", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Agenda).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Version).HasMaxLength(100);
            });

            modelBuilder.Entity<SMImplementation>(entity =>
            {
                entity.ToTable("SMImplementation", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SMDefinitionId).HasColumnName("SMDefinitionId");

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.SMImplementation)
                    .HasForeignKey(d => d.ElementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SMImplementation_Element");

                entity.HasOne(d => d.ElementType)
                    .WithMany(p => p.SMImplementation)
                    .HasForeignKey(d => d.ElementTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SMImplementation_ElementType");

                entity.HasOne(d => d.SMDefinition)
                    .WithMany(p => p.SMImplementation)
                    .HasForeignKey(d => d.SMDefinitionId)
                    .HasConstraintName("FK_SMImplementation_SMDefinition");
            });

            modelBuilder.Entity<TransitionTrigger>(entity =>
            {
                entity.ToTable("TransitionTrigger", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });
        }
    }
}
