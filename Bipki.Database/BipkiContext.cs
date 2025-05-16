using Bipki.Database.Models.BusinessModels;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bipki.Database;

public class BipkiContext : IdentityDbContext<User, Role, Guid>
{
    public BipkiContext(DbContextOptions<BipkiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conference> Conferences { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<ActivityRegistration> ActivityRegistrations { get; set; }

    public virtual DbSet<WaitListEntry> WaitListEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresEnum<ActivityType>("public", "activity_type");

        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.Name).HasColumnName("name")
                .HasMaxLength(50);

            entity.Property(e => e.Surname).HasColumnName("surname")
                .HasMaxLength(50);

            entity.Property(e => e.Telegram).HasColumnName("telegram")
                .HasMaxLength(50);

            entity.Property(e => e.ConferenceId).HasColumnName("conference_id");
        });

        builder.Entity<Conference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conferences_pkey");

            entity.ToTable("conferences");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");

            entity.Property(e => e.LocationId).HasColumnName("location_id");

            entity.Property(e => e.Plan).HasColumnName("floor_plan"); // TODO probably needs change

            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");

            entity.HasOne(c => c.Location).WithMany(l => l.Conferences)
                .HasForeignKey(c => c.LocationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("conferences_location_id_fkey");
        });

        builder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activities_pkey");

            entity.ToTable("activities");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");

            entity.Property(e => e.Type)
                .HasColumnType("activity_type")
                .HasColumnName("type");

            entity.Property(e => e.Recording)
                .HasColumnName("recording");

            entity.Property(e => e.StartsAt)
                .HasColumnName("starts_at");

            entity.Property(e => e.EndsAt)
                .HasColumnName("ends_at");

            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");

            entity.HasIndex(e => e.StartsAt)
                .HasDatabaseName("IX_activities_starts_at");
        });

        builder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("locations_pkey");

            entity.ToTable("locations");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasDefaultValue("deleted");
        });

        builder.Entity<ActivityRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_registration_pkey");

            entity.ToTable("activity_registrations");

            entity.Property(e => e.ActivityId).HasColumnName("activity_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.RegisteredAt).HasColumnName("registered_at");

            entity.Property(e => e.Deleted).HasColumnName("deleted");

            entity.HasOne(e => e.Activity).WithMany(a => a.ActivityRegistrations)
                .HasForeignKey(e => e.ActivityId)
                .HasConstraintName("activity_registrations_activity_id_fkey");

            entity.HasOne(e => e.User).WithMany(u => u.ActivityRegistrations)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("activity_registrations_user_id_fkey");
        });

        builder.Entity<WaitListEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("wait_list_entry_pkey");

            entity.ToTable("wait_list_entries");

            entity.Property(e => e.ActivityId).HasColumnName("activity_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.WaitsSince).HasColumnName("waits_since");

            entity.Property(e => e.Deleted).HasColumnName("deleted");

            entity.HasOne(e => e.Activity).WithMany(a => a.WaitList)
                .HasForeignKey(e => e.ActivityId)
                .HasConstraintName("wait_list_entries_activity_id_fkey");

            entity.HasOne(e => e.User).WithMany(u => u.WaitLists)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("wait_list_entries_user_id_fkey");

            entity.HasIndex(e => new { e.ActivityId, e.WaitsSince })
                .HasDatabaseName("IX_wait_list_entries_activity_id_waits_since");
        });
    }
}