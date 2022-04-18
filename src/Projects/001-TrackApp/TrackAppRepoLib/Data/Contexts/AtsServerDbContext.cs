using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TrackAppRepoLib.Data.Entities;

namespace TrackAppRepoLib.Data.Contexts
{
    public partial class AtsServerDbContext : DbContext
    {
        public AtsServerDbContext()
        {
        }

        public AtsServerDbContext(DbContextOptions<AtsServerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SwitchDatabase> SwitchDatabases { get; set; } = null!;
        public virtual DbSet<TrackDatabase> TrackDatabases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;
       
             optionsBuilder.UseSqlServer("Server = aws-mssql.cct1ehgoywdp.us-east-2.rds.amazonaws.com; Database = metroistanbul_atsserversb; User = admin; Password = csystem1993;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SwitchDatabase>(entity =>
            {
                entity.ToTable("SwitchDatabase");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.EntryTrack).HasColumnName("Entry_Track");

                entity.Property(e => e.LeftTrack).HasColumnName("Left_Track");

                entity.Property(e => e.NormalPosition).HasColumnName("Normal_Position");

                entity.Property(e => e.RightTrack).HasColumnName("Right_Track");

                entity.Property(e => e.StationName).HasColumnName("Station_Name");

                entity.Property(e => e.SwitchId).HasColumnName("Switch_ID");
            });

            modelBuilder.Entity<TrackDatabase>(entity =>
            {
                entity.HasKey(e => e.No)
                    .HasName("PK__TrackDat__3214D4A898A74C20");

                entity.ToTable("TrackDatabase");

                entity.Property(e => e.No).ValueGeneratedNever();

                entity.Property(e => e.LineId).HasColumnName("Line_ID");

                entity.Property(e => e.PsdEsbId1).HasColumnName("PSD_ESB_ID_1");

                entity.Property(e => e.PsdEsbId2).HasColumnName("PSD_ESB_ID_2");

                entity.Property(e => e.SignalId1).HasColumnName("Signal_ID_1");

                entity.Property(e => e.SignalId2).HasColumnName("Signal_ID_2");

                entity.Property(e => e.StationEndPosition).HasColumnName("Station_End_Position");

                entity.Property(e => e.StationName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Station_Name");

                entity.Property(e => e.StationStartPosition).HasColumnName("Station_Start_Position");

                entity.Property(e => e.StoppingPointPosition1).HasColumnName("Stopping_Point_Position_1");

                entity.Property(e => e.StoppingPointPosition2).HasColumnName("Stopping_Point_Position_2");

                entity.Property(e => e.StoppingPointType1).HasColumnName("Stopping_Point_Type_1");

                entity.Property(e => e.StoppingPointType2).HasColumnName("Stopping_Point_Type_2");

                entity.Property(e => e.TrackConnectionEntry1).HasColumnName("Track_Connection_Entry_1");

                entity.Property(e => e.TrackConnectionEntry2).HasColumnName("Track_Connection_Entry_2");

                entity.Property(e => e.TrackConnectionExit1).HasColumnName("Track_Connection_Exit_1");

                entity.Property(e => e.TrackConnectionExit2).HasColumnName("Track_Connection_Exit_2");

                entity.Property(e => e.TrackEndPosition).HasColumnName("Track_End_Position");

                entity.Property(e => e.TrackId).HasColumnName("Track_ID");

                entity.Property(e => e.TrackLength).HasColumnName("Track_Length");

                entity.Property(e => e.TrackSpeedLimit).HasColumnName("Track_Speed_Limit");

                entity.Property(e => e.TrackStartPosition).HasColumnName("Track_Start_Position");

                entity.Property(e => e.TrackType).HasColumnName("Track_Type");

                entity.Property(e => e.WaysideId).HasColumnName("Wayside_ID");

                entity.Property(e => e.X1Point).HasColumnName("X1_Point");

                entity.Property(e => e.X2Point).HasColumnName("X2_Point");

                entity.Property(e => e.Y1Point).HasColumnName("Y1_Point");

                entity.Property(e => e.Y2Point).HasColumnName("Y2_Point");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
