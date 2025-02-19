using HealthPassport.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthPassport.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<FamilyStatus> FamilyStatuses { get; set; }
        public DbSet<AntropologicalResearch> AntropologicalResearches { get; set; }
        public DbSet<Education> Educations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (LocalDB)\\MSSQLLocalDB; Database=HealthPassport;  AttachDbFilename=|DataDirectory|HealthPassport.mdf; Trusted_Connection=True;MultipleActiveResultSets=true; Integrated Security=True;Connect Timeout=30; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Diseases) //Один сотрудник может иметь много заболеваний
                .WithOne(d => d.Employee) //Каждое заболевание связано с одним сотрудником
                .HasForeignKey(d => d.EmployeeId) //Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); //Каскадное удаление

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Vaccinations) //Один сотрудник может иметь много прививок
                .WithOne(v => v.Employee) //Каждая прививка связана с одним сотрудником
                .HasForeignKey(v => v.EmployeeId) //Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); //Каскадное удаление

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Jobs) //Один сотрудник может иметь много должностей
                .WithOne(j => j.Employee) //Каждая должность связана с одним сотрудником
                .HasForeignKey(j => j.EmployeeId) //Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); //Каскадное удаление

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.FamilyStatuses) //Один сотрудник может иметь много семейных статусов
                .WithOne(fs => fs.Employee) //Каждый семейный статус связан с одним сотрудником
                .HasForeignKey(fs => fs.EmployeeId) //Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); //Каскадное удаление

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.AntropologicalResearches) //Один сотрудник может иметь много антропологических исследований
                .WithOne(ar => ar.Employee) //Каждое антропологическое исследование связано с одним сотрудником
                .HasForeignKey(ar => ar.EmployeeId) //Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); //Каскадное удаление

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Educations) //Один сотрудник может иметь много образований
                .WithOne(ed => ed.Employee) //Каждое образование связано с одним сотрудником
                .HasForeignKey(ed => ed.EmployeeId) //Внешний ключ
                .OnDelete(DeleteBehavior.Cascade); //Каскадное удаление

            //Должности
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Employee)
                .WithMany(j => j.Jobs)
                .HasForeignKey(j => j.EmployeeId);

            //Связывание Job и JobType
            modelBuilder.Entity<Job>()
                .HasOne(j => j.JobType)
                .WithMany(j => j.Jobs)
                .HasForeignKey(j => j.JobTypeId);
        }
    }
}
