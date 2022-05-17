using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NursesSheduler.Application;
using NursesSheduler.BlazorShared.Data;
using NursesSheduler.Persistance;
using System.Windows;

namespace NursesSheduler.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddBlazorWebView();
                services.AddSingleton<WeatherForecastService>();
                services.AddPersistenceLayer();
                services.AddApplicationLayer();
            }).Build();

            Resources.Add("services", host.Services);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            host.Start();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            host.Dispose();

            base.OnExit(e);
        }
    }
}
