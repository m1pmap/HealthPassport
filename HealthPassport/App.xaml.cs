using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HealthPassport.BLL.Services;
using HealthPassport.BLL.Interfaces;
using HealthPassport.DAL.Repositories;
using HealthPassport.DAL.Interfaces;
using HealthPassport.Pages;
using Dahmira.Pages;
using HealthPassport.Services;
using HealthPassport.DAL;
using HealthPassport.Interfaces;
using Dahmira.Services;
using Microsoft.EntityFrameworkCore;
using HealthPassport.Models;
using HealthPassport.DAL.Models;

namespace HealthPassport
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceProvider = ConfigureServices();
            Resources["ByteArrayToImageSourceConverter"] = ServiceProvider.GetService<ByteArrayToImageSourceConverter_Service>();
            Resources["NullToEmptyStringConverter"] = ServiceProvider.GetService<NullToEmptyStringConverter>();
            ServiceLocator.Provider = ServiceProvider;

            // 🔹 Запускаем LoginPage
            var loginPage = ServiceProvider.GetRequiredService<LoginPage>();
            loginPage.Show();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //Регистрация Converter'ов
            services.AddSingleton<ByteArrayToImageSourceConverter_Service>();
            services.AddSingleton<NullToEmptyStringConverter>();
            services.AddScoped<IImageUpdater, ImageUpdater_Service>();
            services.AddScoped<IFileWorker, FileWorker_Service>();
            services.AddKeyedScoped<IExportWorker, ExcelWorker_Service>("Excel");
            services.AddKeyedScoped<IExportWorker, WordWorker_Service>("Word");
            services.AddScoped<IShaderEffects, ShaderEffects_Service>();

            //Регистрация BLL сервисов
            services.AddScoped<IMailSender, MailSender_Service>();
            services.AddScoped<IJsonSerializer, JsonSerializer_Service>();

            services.AddScoped<IEmployeeProcessing, EmployeeProcessing_Service>();
            services.AddScoped<ISubunitProcessing, SubunitProcessing_Service>();
            services.AddScoped<IJobTypeProcessing, JobTypeProcessing_Service>();
            services.AddScoped<IAuditLogProcessing, AuditLogProcessing_Service>();

            services.AddScoped<ICudProcessing<Job>, JobProcessing_Service>();
            //services.AddScoped<IGetProcessing<JobType>, JobTypeProcessing_Service>();
            services.AddScoped<ICudProcessing<Education>, EducationProcessing_Service>();
            services.AddScoped<IGetProcessing<EducationLevel>, EducationLevelProcessing_Service>();
            services.AddScoped<ICudProcessing<AntropologicalResearch>, AntropologicalResearchProcessing_Service>();
            services.AddScoped<IDiseaseProcessing, DiseaseProcessing_Service>();
            services.AddScoped<ICudProcessing<Vaccination>, VaccinationProcessing_Service>();
            services.AddScoped<ICudProcessing<FamilyStatus>, FamilyStatusProcessing_Service>();

            //Регистрация DAL репозиториев
            services.AddScoped<IEmployee, Employee_Repository>();
            services.AddScoped<IDisease, Disease_Repository>();
            services.AddScoped<ISubunit, Subunit_Repository>();
            services.AddScoped<IJobType, JobType_Repository>();
            services.AddScoped<IAuditLog, AuditLog_Repository>();

            services.AddScoped(typeof(ICudRepository<Vaccination>), typeof(Vaccination_Repository));
            services.AddScoped(typeof(ICudRepository<Job>), typeof(Job_Repository));
            //services.AddScoped(typeof(IGetRepository<JobType>), typeof(JobType_Repository));
            services.AddScoped(typeof(ICudRepository<FamilyStatus>), typeof(FamilyStatus_Repository));
            services.AddScoped(typeof(ICudRepository<AntropologicalResearch>), typeof(AntropologicalResearch_Repository));
            services.AddScoped(typeof(ICudRepository<Education>), typeof(Education_Repository));
            //services.AddScoped(typeof(IGetRepository<Subunit>), typeof(Subunit_Repository));
            services.AddScoped(typeof(IGetRepository<EducationLevel>), typeof(EducationLevel_Repository));

            //Регистрация контекста БД
            services.AddDbContext<ApplicationDbContext>();

            //Регистрация окон
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<WriteMailCodePage>();
            services.AddTransient<MoreEmployeeInfoPage>();
            services.AddTransient<VaccnationsAddPage>();
            services.AddTransient<RefactoredEmployeesPage>();
            services.AddTransient<FastSearchPage>();
            services.AddTransient<SettingsPage>();


            return services.BuildServiceProvider();
        }
    }
}
