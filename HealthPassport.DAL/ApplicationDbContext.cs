using HealthPassport.DAL.Interfaces;
using HealthPassport.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthPassport.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public int CurrentEmployeeid = 0;
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<FamilyStatus> FamilyStatuses { get; set; }
        public DbSet<AntropologicalResearch> AntropologicalResearches { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Subunit> Subunits { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

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

            //Связывание Job и Subunit
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Subunit)
                .WithMany(s => s.Jobs)
                .HasForeignKey(s => s.SubunitId);

            //Связывание Education и EducationLevel
            modelBuilder.Entity<Education>()
                .HasOne(e => e.EducationLevel)
                .WithMany(el => el.Educations)
                .HasForeignKey(el => el.EducationLevelId);

            //AuditLog для хранения изменений в БД
            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");
            base.OnModelCreating(modelBuilder);
        }




        //Триггеры при изменении
        private readonly List<AuditLog> _pendingAuditLogs = new();

        public override int SaveChanges()
        {
            PrepareAuditLogs();
            var result = base.SaveChanges(); // сохраняем основные изменения

            FinalizeAuditLogs(); // получаем настоящие ключи
            if (_pendingAuditLogs.Any())
            {
                AuditLogs.AddRange(_pendingAuditLogs);
                base.SaveChanges(); // сохраняем аудит
                _pendingAuditLogs.Clear();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            PrepareAuditLogs();
            var result = await base.SaveChangesAsync(cancellationToken);

            FinalizeAuditLogs();
            if (_pendingAuditLogs.Any())
            {
                AuditLogs.AddRange(_pendingAuditLogs);
                await base.SaveChangesAsync(cancellationToken);
                _pendingAuditLogs.Clear();
            }

            return result;
        }

        private void PrepareAuditLogs()
        {
            _pendingAuditLogs.Clear();

            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                var audit = new AuditLog
                {
                    TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                    Action = entry.State.ToString().ToUpper(),
                    ChangedAt = DateTime.Now,
                    ChangedBy = GetCurrentUser(),
                    Tag = entry
                };

                if (entry.State == EntityState.Modified)
                {
                    var changedProps = entry.Properties.Where(p => p.IsModified).ToList();
                    audit.ChangedColumns = string.Join(", ", changedProps.Select(p => p.Metadata.Name));
                    audit.OldValues = Serialize(changedProps.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
                    audit.NewValues = Serialize(changedProps.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
                }
                else if (entry.State == EntityState.Added)
                {
                    //audit.NewValues = Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue));
                }
                else if (entry.State == EntityState.Deleted)
                {
                    audit.OldValues = Serialize(entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue));
                }

                _pendingAuditLogs.Add(audit);
            }
        }

        private void FinalizeAuditLogs()
        {
            foreach (var audit in _pendingAuditLogs)
            {
                if (audit.Tag is EntityEntry entry)
                {
                    audit.PrimaryKey = GetPrimaryKeyValue(entry);
                    audit.Tag = null; // очищаем лишнее
                }
            }
        }

        private string GetPrimaryKeyValue(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();
            if (key == null) return string.Empty;

            var keyValues = key.Properties
                .Select(p => entry.Property(p.Name).CurrentValue?.ToString());

            return string.Join(",", keyValues);
        }

        private string Serialize(object obj) =>
            JsonSerializer.Serialize(obj);

        private string? GetCurrentUser()
        {
            string fio = Employees.First(e => e.EmployeeId == CurrentEmployeeid).FIO;
            var splitFio = fio.Split(' ');

            return $"{splitFio[0]} {splitFio[1][0]}. {splitFio[2][0]}.";
            // Реализуй по-своему (из контекста, токена, потока и т.д.)
            //return "system";
        }
    }
}
