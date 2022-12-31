using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using NursesScheduler.WPF.Stores;
using NursesScheduler.WPF.ViewModels;
using System;
using System.Threading;
using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.Services.Implementation;

namespace NursesScheduler.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                //services
                services.AddScoped<IDatabaseService, DatabaseService>();
                services.AddScoped<IPasswordService, PasswordService>();

                //stores
                services.AddSingleton<NavigationStore>();

                //main window
                services.AddSingleton<MainViewModel>();
                services.AddSingleton(s => new AuthorizationWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });

                //viewmodels
                services.AddTransient<LogInViewModel>();
                services.AddSingleton<Func<LogInViewModel>>((s) => () => s.GetRequiredService<LogInViewModel>());
                services.AddSingleton<NavigationService<LogInViewModel>>();

                services.AddTransient<ChangePasswordViewModel>();
                services.AddSingleton<Func<ChangePasswordViewModel>>((s) => () => s.GetRequiredService<ChangePasswordViewModel>());
                services.AddSingleton<NavigationService<ChangePasswordViewModel>>();

                services.AddTransient<RegisterViewModel>();
                services.AddSingleton<Func<RegisterViewModel>>((s) => () => s.GetRequiredService<RegisterViewModel>());
                services.AddSingleton<NavigationService<RegisterViewModel>>();

                services.AddTransient<SettingsViewModel>();
                services.AddSingleton<Func<SettingsViewModel>>((s) => () => s.GetRequiredService<SettingsViewModel>());
                services.AddSingleton<NavigationService<SettingsViewModel>>();

            }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            SetLanguageDictionary();

            _host.Start();

            IDatabaseService databaseManager = _host.Services.GetRequiredService<IDatabaseService>();

            var isDbCreated = databaseManager.IsDbCreated();

            INavigationService navigationService;

            if (isDbCreated)
            {
                navigationService = _host.Services.GetRequiredService<NavigationService<LogInViewModel>>();
            }
            else
            {
                navigationService = _host.Services.GetRequiredService<NavigationService<RegisterViewModel>>();
            }

            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<AuthorizationWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();

            base.OnExit(e);
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
        
        public void DisposeHost()
        {
            _host.Dispose();
           this.MainWindow.Close();
        }
    }
}
