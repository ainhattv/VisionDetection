﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VDS.BlobService.Data;

namespace BlobService.Data.Migrations
{
    [DbContext(typeof(BlobContext))]
    partial class BlobContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VDS.BlobService.Data.Entities.BlobContainer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<Guid>("WorkPlaceId");

                    b.HasKey("Id");

                    b.ToTable("BlobContainers");
                });

            modelBuilder.Entity("VDS.BlobService.Data.Entities.BlobFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("BlobContainerId");

                    b.Property<bool>("IsActive");

                    b.HasKey("Id");

                    b.HasIndex("BlobContainerId");

                    b.ToTable("BlobFolders");
                });

            modelBuilder.Entity("VDS.BlobService.Data.Entities.BlobFolder", b =>
                {
                    b.HasOne("VDS.BlobService.Data.Entities.BlobContainer", "BlobContainer")
                        .WithMany("BlobFolders")
                        .HasForeignKey("BlobContainerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}