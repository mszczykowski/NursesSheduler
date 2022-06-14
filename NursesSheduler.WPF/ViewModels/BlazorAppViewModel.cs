using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NursesScheduler.BlazorShared.Data;
using NursesScheduler.BusinessLogic;
using NursesScheduler.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

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
                services.AddPersistenceLayer(connectionString);
            }).Build();

            _host.Start();
        }
    }
}
