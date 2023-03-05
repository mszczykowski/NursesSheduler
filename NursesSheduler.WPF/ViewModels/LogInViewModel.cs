using NursesScheduler.WPF.Commands;
using NursesScheduler.WPF.Commands.Common;
using NursesScheduler.WPF.Helpers;
using NursesScheduler.WPF.Models.Enums;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.Services.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace NursesScheduler.WPF.ViewModels
{
    internal sealed class LogInViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _password;
        public string Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _savePassword;
        public bool SavePassword 
        { 
            get => _savePassword; 
            set
            {
                _savePassword = value;
                OnPropertyChanged(nameof(SavePassword));
            }
        }

        public ICommand NavigateToSettingsCommand { get; set; }
        public ICommand NavigateToChangePasswordCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private readonly IDatabaseService _databaseService;

        public LogInViewModel(NavigationService<SettingsViewModel> settingsViewNavigationService,
            NavigationService<ChangePasswordViewModel> changePasswordViewNavigationService,
            IDatabaseService databaseService, IPasswordService passwordService)
        {
             _databaseService = databaseService;

            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(settingsViewNavigationService);

            NavigateToChangePasswordCommand = new NavigateCommand<ChangePasswordViewModel>(changePasswordViewNavigationService);

            LogInCommand = new LogInCommand(this, _databaseService, passwordService);

            ExitCommand = new ExitCommand();

            _errorsViewModel = new ErrorsViewModel();

            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;

            TryLoadPassword(passwordService);
        }

        private readonly ErrorsViewModel _errorsViewModel;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errorsViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public void ValidateForm()
        {
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            _errorsViewModel.ClearErrors(nameof(Password));

            if (string.IsNullOrEmpty(Password))
                _errorsViewModel.AddError(nameof(Password),
                    PasswordValidationMessageHelper.GetValidationMessage(PasswordValidationResult.Empty));
        }

        private void TryLoadPassword(IPasswordService passwordService)
        {
            var password = passwordService.TryRetrievePassword();

            if (String.IsNullOrEmpty(password)) return;

            Password = password;
            SavePassword = true;
        }
    }
}
