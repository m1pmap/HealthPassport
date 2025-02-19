﻿// <auto-generated />
using System;
using HealthPassport.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthPassport.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250219152700_updateEmployeeTable")]
    partial class updateEmployeeTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HealthPassport.DAL.Models.AntropologicalResearch", b =>
                {
                    b.Property<int>("AntropologicalResearchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AntropologicalResearchId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<double>("Height")
                        .HasColumnType("float");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("AntropologicalResearchId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("AntropologicalResearches");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Disease", b =>
                {
                    b.Property<int>("DiseaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiseaseId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDiseaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime>("StardDiseaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DiseaseId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Diseases");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Education", b =>
                {
                    b.Property<int>("EducationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EducationId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("EducationInstitution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EducationType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("EducationId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("FIO")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("MailAdress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Photo")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.FamilyStatus", b =>
                {
                    b.Property<int>("FamilyStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FamilyStatusId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndFamilyDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartFamilyDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FamilyStatusId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("FamilyStatuses");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Job", b =>
                {
                    b.Property<int>("JobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndWorkingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("JobTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartWorkingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subunit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WorkingRate")
                        .HasColumnType("float");

                    b.HasKey("JobId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("JobTypeId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.JobType", b =>
                {
                    b.Property<int>("JobTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobTypeId"));

                    b.Property<string>("JobName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isCanAddRows")
                        .HasColumnType("bit");

                    b.Property<bool>("isCanSendMessages")
                        .HasColumnType("bit");

                    b.HasKey("JobTypeId");

                    b.ToTable("JobTypes");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Vaccination", b =>
                {
                    b.Property<int>("VaccinationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VaccinationId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VaccinationId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Vaccinations");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.AntropologicalResearch", b =>
                {
                    b.HasOne("HealthPassport.DAL.Models.Employee", "Employee")
                        .WithMany("AntropologicalResearches")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Disease", b =>
                {
                    b.HasOne("HealthPassport.DAL.Models.Employee", "Employee")
                        .WithMany("Diseases")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Education", b =>
                {
                    b.HasOne("HealthPassport.DAL.Models.Employee", "Employee")
                        .WithMany("Educations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.FamilyStatus", b =>
                {
                    b.HasOne("HealthPassport.DAL.Models.Employee", "Employee")
                        .WithMany("FamilyStatuses")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Job", b =>
                {
                    b.HasOne("HealthPassport.DAL.Models.Employee", "Employee")
                        .WithMany("Jobs")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthPassport.DAL.Models.JobType", "JobType")
                        .WithMany("Jobs")
                        .HasForeignKey("JobTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("JobType");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Vaccination", b =>
                {
                    b.HasOne("HealthPassport.DAL.Models.Employee", "Employee")
                        .WithMany("Vaccinations")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.Employee", b =>
                {
                    b.Navigation("AntropologicalResearches");

                    b.Navigation("Diseases");

                    b.Navigation("Educations");

                    b.Navigation("FamilyStatuses");

                    b.Navigation("Jobs");

                    b.Navigation("Vaccinations");
                });

            modelBuilder.Entity("HealthPassport.DAL.Models.JobType", b =>
                {
                    b.Navigation("Jobs");
                });
#pragma warning restore 612, 618
        }
    }
}
