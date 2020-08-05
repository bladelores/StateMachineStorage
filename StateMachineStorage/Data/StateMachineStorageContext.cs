using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using StateMachineStorage.Data.Models;

namespace StateMachineStorage.Data
{
    public partial class StateMachineStorageContext : DbContext
    {
        public StateMachineStorageContext()
        {
        }

        public StateMachineStorageContext(DbContextOptions<StateMachineStorageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EventLog> EventLog { get; set; }
        public virtual DbSet<SMDefinition> SMDefinition { get; set; }
        public virtual DbSet<SMImplementation> SMImplementation { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Transition> Transition { get; set; }
        public virtual DbSet<TransitionTrigger> TransitionTrigger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connection = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

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

                entity.HasOne(d => d.InitialState)
                    .WithMany(p => p.SMDefinition)
                    .HasForeignKey(d => d.InitialStateId)
                    .HasConstraintName("FK_SMDefinition_State");
            });

            modelBuilder.Entity<SMImplementation>(entity =>
            {
                entity.ToTable("SMImplementation", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SMDefinitionId).HasColumnName("SMDefinitionId");

                entity.HasOne(d => d.SMDefinition)
                    .WithMany(p => p.SMImplementation)
                    .HasForeignKey(d => d.SMDefinitionId)
                    .HasConstraintName("FK_SMImplementation_SMDefinition");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.SMImplementation)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SMImplementation_State");

                entity.HasOne(d => d.Transition)
                    .WithMany(p => p.SMImplementation)
                    .HasForeignKey(d => d.TransitionId)
                    .HasConstraintName("FK_SMImplementation_Transition");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SMDefinitionId).HasColumnName("SMDefinitionId");

                entity.HasOne(d => d.SMDefinitionNavigation)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.SMDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_State_SMDefinition");
            });

            modelBuilder.Entity<Transition>(entity =>
            {
                entity.ToTable("Transition", "STATE_MACHINE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SMDefinitionId).HasColumnName("SMDefinitionId");

                entity.HasOne(d => d.NewState)
                    .WithMany(p => p.TransitionNewState)
                    .HasForeignKey(d => d.NewStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transition_StateNew");

                entity.HasOne(d => d.OldState)
                    .WithMany(p => p.TransitionOldState)
                    .HasForeignKey(d => d.OldStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transition_StateOld");

                entity.HasOne(d => d.SMDefinition)
                    .WithMany(p => p.Transition)
                    .HasForeignKey(d => d.SMDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transition_SMDefinition");

                entity.HasOne(d => d.TransitionTrigger)
                    .WithMany(p => p.Transition)
                    .HasForeignKey(d => d.TransitionTriggerId)
                    .HasConstraintName("FK_Transition_TransitionTrigger");
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
