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
            services.AddScoped<IShaderEffects, ShaderEffects_Service>();
            //Регистрация BLL сервисов
            services.AddScoped<IMailSender, MailSender_Service>();
            services.AddScoped<IEmployeeProcessing, EmployeeProcessing_Service>();
            services.AddScoped<IJsonSerializer, JsonSerializer_Service>();

            //Регистрация DAL репозиториев
            services.AddScoped<IEmployee, Employee_Repository>();

            //Регистрация контекста БД
            services.AddDbContext<ApplicationDbContext>();

            //Регистрация окон
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginPage>();
            services.AddTransient<WriteMailCodePage>();
            services.AddTransient<MoreEmployeeInfoPage>();

            return services.BuildServiceProvider();
        }
    }
}
