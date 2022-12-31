using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NursesScheduler.BlazorShared.Data;
using NursesScheduler.BusinessLogic;
using NursesScheduler.Infrastructure;
using System;

namespace NursesScheduler.WPF.ViewModels
{
    internal class BlazorAppViewModel
    {
        public IServiceProvider Services => _host.Services;

        private readonly IHost _host;

        public BlazorAppViewModel(string connectionString)
        {

            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddBlazorWebView();
                services.AddSingleton<WeatherForecastService>();
                services.AddBusinessLogicLayer();
                services.AddInfrastructureLayer(connectionString);
            }).Build();

            _host.Start();
        }
    }
}
