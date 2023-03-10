// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NAPApi.Context;

#nullable disable

namespace NAPApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NAPApi.Entity.Files", b =>
                {
                    b.Property<int>("FilesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FilesId"));

                    b.Property<DateTime>("FileCreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FileIdUses")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("FilesId");

                    b.HasIndex("GroupId");

                    b.ToTable("files");
                });

            modelBuilder.Entity("NAPApi.Entity.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("groups");
                });

            modelBuilder.Entity("NAPApi.Entity.Logging", b =>
                {
                    b.Property<int>("LoggingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoggingId"));

                    b.Property<string>("LoggingAction")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoggingId");

                    b.HasIndex("UserId");

                    b.ToTable("loggings");
                });

            modelBuilder.Entity("NAPApi.Entity.PermessionsGroup", b =>
                {
                    b.Property<int>("PermessionsGroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermessionsGroupId"));

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("PermessionsGroupSharedId")
                        .HasColumnType("int");

                    b.HasKey("PermessionsGroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("permessionsGroups");
                });

            modelBuilder.Entity("NAPApi.Entity.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReportId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReportId");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("reports");
                });

            modelBuilder.Entity("NAPApi.Entity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("NAPApi.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Confirm")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("NAPApi.Entity.Files", b =>
                {
                    b.HasOne("NAPApi.Entity.Group", "Groups")
                        .WithMany("files")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Groups");
                });

            modelBuilder.Entity("NAPApi.Entity.Group", b =>
                {
                    b.HasOne("NAPApi.Entity.User", "User")
                        .WithMany("groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NAPApi.Entity.Logging", b =>
                {
                    b.HasOne("NAPApi.Entity.User", "User")
                        .WithMany("Loggings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NAPApi.Entity.PermessionsGroup", b =>
                {
                    b.HasOne("NAPApi.Entity.Group", "Group")
                        .WithMany("permessionsGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("NAPApi.Entity.Report", b =>
                {
                    b.HasOne("NAPApi.Entity.Files", "File")
                        .WithMany("reports")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NAPApi.Entity.User", "User")
                        .WithMany("Reports")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NAPApi.Entity.User", b =>
                {
                    b.HasOne("NAPApi.Entity.Role", "Role")
                        .WithMany("users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("NAPApi.Entity.Files", b =>
                {
                    b.Navigation("reports");
                });

            modelBuilder.Entity("NAPApi.Entity.Group", b =>
                {
                    b.Navigation("files");

                    b.Navigation("permessionsGroups");
                });

            modelBuilder.Entity("NAPApi.Entity.Role", b =>
                {
                    b.Navigation("users");
                });

            modelBuilder.Entity("NAPApi.Entity.User", b =>
                {
                    b.Navigation("Loggings");

                    b.Navigation("Reports");

                    b.Navigation("groups");
                });
#pragma warning restore 612, 618
        }
    }
}
