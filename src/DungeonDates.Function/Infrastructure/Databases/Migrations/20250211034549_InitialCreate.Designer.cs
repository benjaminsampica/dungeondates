﻿// <auto-generated />
using System;
using DungeonDates.Function.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DungeonDates.Function.Infrastructure.Databases.Migrations
{
    [DbContext(typeof(DungeonDatesDbContext))]
    [Migration("20250211034549_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.DungeonDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("DungeonDates");
                });

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.ProposedDate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DungeonDateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DungeonDateId");

                    b.ToTable("ProposedDates");
                });

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.ProposedDateResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Accepted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("ProposedDateId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProposedDateId");

                    b.ToTable("ProposedDateResponses");
                });

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.ProposedDate", b =>
                {
                    b.HasOne("DungeonDates.Function.Infrastructure.Databases.DungeonDate", "DungeonDate")
                        .WithMany("Dates")
                        .HasForeignKey("DungeonDateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DungeonDate");
                });

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.ProposedDateResponse", b =>
                {
                    b.HasOne("DungeonDates.Function.Infrastructure.Databases.ProposedDate", null)
                        .WithMany("Responses")
                        .HasForeignKey("ProposedDateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.DungeonDate", b =>
                {
                    b.Navigation("Dates");
                });

            modelBuilder.Entity("DungeonDates.Function.Infrastructure.Databases.ProposedDate", b =>
                {
                    b.Navigation("Responses");
                });
#pragma warning restore 612, 618
        }
    }
}
