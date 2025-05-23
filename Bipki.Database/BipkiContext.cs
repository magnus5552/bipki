﻿using Bipki.Database.Models.BusinessModels;
using Bipki.Database.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bipki.Database;

public class BipkiContext : IdentityDbContext<User, Role, Guid>
{
    public BipkiContext(DbContextOptions<BipkiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conference> Conferences { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityRegistration> ActivityRegistrations { get; set; }

    public virtual DbSet<WaitListEntry> WaitListEntries { get; set; }
    
    public virtual DbSet<Chat> Chats { get; set; }
    
    public virtual DbSet<Message> Messages { get; set; }
    
    public virtual DbSet<Poll> Polls { get; set; }
    
    public virtual DbSet<PollOption> PollOptions { get; set; }
    
    public virtual DbSet<Vote> Votes { get; set; }
    
    public virtual DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

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

            entity.Property(e => e.CheckedIn).HasColumnName("checked_in")
                .HasDefaultValue(false);

            entity.HasData(new User
            {
                Id = Guid.Parse("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                Name = "admin",
                Surname = "admin",
                UserName = "admin",
                Telegram = "adminTg",
                NormalizedUserName = "ADMIN",
                SecurityStamp = "oikamsda"
            });
        });

        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.HasData(new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                    RoleId = Guid.Parse("5e2b6f6f-a877-46ca-9b82-cc7d6a4118d5")
                },
                new IdentityUserRole<Guid>
                {
                    UserId = Guid.Parse("7e0ca8d7-841b-4f0d-92e8-64ed6dd9805a"),
                    RoleId = Guid.Parse("4bcb87c6-3320-4de0-8e7c-c6765a08916b")
                });
        });

        builder.Entity<Conference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conferences_pkey");

            entity.ToTable("conferences");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.Plan).HasColumnName("floor_plan"); // TODO probably needs change

            entity.Property(e => e.Location).HasColumnName("location");

            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.Property(e => e.EndDate).HasColumnName("end_date");

            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");
            
            entity.HasOne(e => e.Chat)
                .WithOne(e => e.Conference)
                .HasForeignKey<Conference>(e => e.ChatId)
                .HasConstraintName("activities_chat_id_fkey");
        });

        builder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activities_pkey");

            entity.ToTable("activities");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            
            entity.Property(e => e.TypeLabel)
                .HasMaxLength(50)
                .HasColumnName("type_label");

            entity.Property(e => e.Description)
                .HasColumnName("description");

            entity.Property(e => e.Type)
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
            
            entity.Property(e => e.ChatId)
                .HasColumnName("chat_id");

            entity.HasOne(e => e.Conference).WithMany(c => c.Program)
                .HasForeignKey(e => e.ConferenceId)
                .HasConstraintName("activities_conference_id_fkey");

            entity.HasOne(e => e.Chat)
                .WithOne(e => e.Activity)
                .HasForeignKey<Activity>(e => e.ChatId)
                .HasConstraintName("activities_chat_id_fkey");
            
            entity.HasIndex(e => e.StartsAt)
                .HasDatabaseName("IX_activities_starts_at");
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

        builder.Entity<Role>(entity =>
        {
            entity.HasData(new Role
            {
                Id = Guid.Parse("4bcb87c6-3320-4de0-8e7c-c6765a08916b"),
                Name = Bipki.Database.Models.UserModels.Roles.User,
                NormalizedName = Bipki.Database.Models.UserModels.Roles.User.Normalize().ToUpperInvariant(),
            }, new Role
            {
                Id = Guid.Parse("5e2b6f6f-a877-46ca-9b82-cc7d6a4118d5"),
                Name = Bipki.Database.Models.UserModels.Roles.Admin,
                NormalizedName = Bipki.Database.Models.UserModels.Roles.Admin.Normalize().ToUpperInvariant(),
            });
        });

        builder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("chat_pkey");

            entity.ToTable("chat");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Type).HasColumnName("type");

            entity.Property(e => e.Title).HasColumnName("title");
        });
        
        builder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ChatId })
                .HasName("chat_user_pkey");

            entity.ToTable("chat_user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("chat_user_user_id_fkey");

            entity.HasOne(d => d.Chat).WithMany()
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("chat_user_chat_id_fkey");
        });
        
        
        builder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("message_pkey");

            entity.ToTable("messages");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Timestamp)
                .HasColumnName("timestamp");

            entity.Property(e => e.Text)
                .HasColumnName("text");

            entity.Property(e => e.SenderId)
                .HasColumnName("sender_id");

            entity.Property(e => e.SenderName)
                .HasColumnName("sender_name");
            
            entity.HasOne(e => e.Chat)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.ChatId)
                .HasConstraintName("messages_chat_id_fkey");

            entity.HasIndex(e => e.ChatId);
        });

        builder.Entity<Poll>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("polls");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.ChatId).HasColumnName("chat_id");

            entity.Property(e => e.Title).HasColumnName("title");

            entity.Property(e => e.TimeStamp).HasColumnName("timestamp");

            entity.HasOne(e => e.Chat)
                .WithMany(e => e.Polls)
                .HasForeignKey(e => e.ChatId)
                .HasConstraintName("polls_chat_id_fkey");
        });

        builder.Entity<PollOption>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("poll_options");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Text).HasColumnName("text");

            entity.Property(e => e.PollId).HasColumnName("poll_id");

            entity.HasOne(e => e.Poll)
                .WithMany(e => e.PollOptions)
                .HasForeignKey(e => e.PollId)
                .HasConstraintName("poll_options_poll_id_fkey");
        });

        builder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("votes");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.PollOptionId).HasColumnName("poll_option_id");

            entity.HasOne(e => e.User)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("votes_user_id_fkey");

            entity.HasOne(e => e.PollOption)
                .WithMany(e => e.Votes)
                .HasForeignKey(e => e.PollOptionId)
                .HasConstraintName("vote_poll_option_id_fkey");
        });

        builder.Entity<NotificationSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("notification_subscriptions");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.SubscribentId).HasColumnName("subscribent_id");

            entity.Property(e => e.PushEndpoint).HasColumnName("endpoint");
            
            entity.Property(e => e.PushAuth).HasColumnName("auth");
            
            entity.Property(e => e.PushP256DH).HasColumnName("p256dh");

            entity.HasOne(e => e.Subscribent).WithMany(e => e.NotificationSubscriptions)
                .HasForeignKey(e => e.SubscribentId)
                .HasConstraintName("notification_subscription_subscribent_id_fkey");
        });
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseEnumCheckConstraints();
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
}