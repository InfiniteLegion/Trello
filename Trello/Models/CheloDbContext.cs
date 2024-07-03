using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trello.Models;

public partial class CheloDbContext : DbContext
{
    public CheloDbContext()
    {
    }

    public CheloDbContext(DbContextOptions<CheloDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardComment> CardComments { get; set; }

    public virtual DbSet<CardTag> CardTags { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<StatusColumn> StatusColumns { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamUser> TeamUsers { get; set; }

    public virtual DbSet<TeamUserNotification> TeamUserNotifications { get; set; }

    public virtual DbSet<UserCard> UserCards { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=chelo_db;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.Boards)
                .HasForeignKey(d => d.IdTeam)
                .HasConstraintName("board_id_team_fkey");
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("card_pkey");

            entity.ToTable("card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Deadline).HasColumnName("deadline");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Label)
                .HasMaxLength(255)
                .HasColumnName("label");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.IdBoard)
                .HasConstraintName("card_id_board_fkey");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Cards)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("card_id_status_fkey");
        });

        modelBuilder.Entity<CardComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("card_comment_pkey");

            entity.ToTable("card_comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommentDatetime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("comment_datetime");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity.Property(e => e.GuidUser)
                .HasMaxLength(55)
                .HasColumnName("guid_user");
            entity.Property(e => e.IdCard).HasColumnName("id_card");

            entity.HasOne(d => d.GuidUserNavigation).WithMany(p => p.CardComments)
                .HasPrincipalKey(p => p.Guid)
                .HasForeignKey(d => d.GuidUser)
                .HasConstraintName("card_comment_guid_user_fkey");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.CardComments)
                .HasForeignKey(d => d.IdCard)
                .HasConstraintName("card_comment_id_card_fkey");
        });

        modelBuilder.Entity<CardTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("card_tags_pkey");

            entity.ToTable("card_tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.IdTags).HasColumnName("id_tags");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.CardTags)
                .HasForeignKey(d => d.IdCard)
                .HasConstraintName("card_tags_id_card_fkey");

            entity.HasOne(d => d.IdTagsNavigation).WithMany(p => p.CardTags)
                .HasForeignKey(d => d.IdTags)
                .HasConstraintName("card_tags_id_tags_fkey");
        });

        modelBuilder.Entity<Configuration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("configuration_pkey");

            entity.ToTable("configuration");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GuidUser)
                .HasMaxLength(55)
                .HasColumnName("guid_user");
            entity.Property(e => e.IsprivateTeamNotifications)
                .HasDefaultValue(false)
                .HasColumnName("isprivate_team_notifications");

            entity.HasOne(d => d.GuidUserNavigation).WithMany(p => p.Configurations)
                .HasPrincipalKey(p => p.Guid)
                .HasForeignKey(d => d.GuidUser)
                .HasConstraintName("configuration_guid_user_fkey");
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friendship_pkey");

            entity.ToTable("friendship");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdUser1Sender).HasColumnName("id_user1_sender");
            entity.Property(e => e.IdUser2Receiver).HasColumnName("id_user2_receiver");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");

            entity.HasOne(d => d.IdUser1SenderNavigation).WithMany(p => p.FriendshipIdUser1SenderNavigations)
                .HasForeignKey(d => d.IdUser1Sender)
                .HasConstraintName("friendship_id_user1_sender_fkey");

            entity.HasOne(d => d.IdUser2ReceiverNavigation).WithMany(p => p.FriendshipIdUser2ReceiverNavigations)
                .HasForeignKey(d => d.IdUser2Receiver)
                .HasConstraintName("friendship_id_user2_receiver_fkey");
        });

        modelBuilder.Entity<StatusColumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status_column");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('status_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.StatusColumns)
                .HasForeignKey(d => d.IdBoard)
                .HasConstraintName("status_column_id_board_fkey");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdBoard).HasColumnName("id_board");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.IdBoardNavigation).WithMany(p => p.Tags)
                .HasForeignKey(d => d.IdBoard)
                .HasConstraintName("tags_id_board_fkey");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("task");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCard).HasColumnName("id_card");
            entity.Property(e => e.Iscompleted)
                .HasDefaultValue(false)
                .HasColumnName("iscompleted");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdCard)
                .HasConstraintName("task_id_card_fkey");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TeamUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_user_pkey");

            entity.ToTable("team_user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTeam).HasColumnName("id_team");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Role)
                .HasMaxLength(25)
                .HasDefaultValueSql("'USER'::character varying")
                .HasColumnName("role");

            entity.HasOne(d => d.IdTeamNavigation).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.IdTeam)
                .HasConstraintName("team_user_id_team_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TeamUsers)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("team_user_id_user_fkey");
        });

        modelBuilder.Entity<TeamUserNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_user_notifications_pkey");

            entity.ToTable("team_user_notifications");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdReceiver).HasColumnName("id_receiver");
            entity.Property(e => e.IdSender).HasColumnName("id_sender");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");

            entity.HasOne(d => d.IdReceiverNavigation).WithMany(p => p.TeamUserNotifications)
                .HasForeignKey(d => d.IdReceiver)
                .HasConstraintName("team_user_notifications_id_receiver_fkey");

            entity.HasOne(d => d.IdSenderNavigation).WithMany(p => p.TeamUserNotifications)
                .HasForeignKey(d => d.IdSender)
                .HasConstraintName("team_user_notifications_id_sender_fkey");
        });

        modelBuilder.Entity<UserCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_card_pkey");

            entity.ToTable("user_card");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GuidUser)
                .HasMaxLength(55)
                .HasColumnName("guid_user");
            entity.Property(e => e.IdCard).HasColumnName("id_card");

            entity.HasOne(d => d.GuidUserNavigation).WithMany(p => p.UserCards)
                .HasPrincipalKey(p => p.Guid)
                .HasForeignKey(d => d.GuidUser)
                .HasConstraintName("user_card_guid_user_fkey");

            entity.HasOne(d => d.IdCardNavigation).WithMany(p => p.UserCards)
                .HasForeignKey(d => d.IdCard)
                .HasConstraintName("user_card_id_card_fkey");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_info_pkey");

            entity.ToTable("user_info");

            entity.HasIndex(e => e.Email, "user_info_email_key").IsUnique();

            entity.HasIndex(e => e.Guid, "user_info_guid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Guid)
                .HasMaxLength(55)
                .HasColumnName("guid");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
