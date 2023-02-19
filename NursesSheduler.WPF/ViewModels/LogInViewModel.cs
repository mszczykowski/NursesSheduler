using NursesScheduler.WPF.Commands;
using NursesScheduler.WPF.Commands.Common;
using NursesScheduler.WPF.Helpers;
using NursesScheduler.WPF.Models.Enums;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.Validators;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace NursesScheduler.WPF.ViewModels
{
    internal class LogInViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _password;
        public string Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                ValidatePassword();
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

        public LogInViewModel(NavigationService<SettingsViewModel> settingsViewNavigationService,
            NavigationService<ChangePasswordViewModel> changePasswordViewNavigationService,
            IDatabaseService databaseManager, IPasswordService passwordService)
        {
            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(settingsViewNavigationService);

            NavigateToChangePasswordCommand = new NavigateCommand<ChangePasswordViewModel>(changePasswordViewNavigationService);

            LogInCommand = new LogInCommand(this, databaseManager, passwordService);

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

            var validationResult = PasswordValidator.ValidatePassword(Password);

            if (validationResult != PasswordValidationResult.Valid) 
                _errorsViewModel.AddError(nameof(Password), PasswordValidationMessageHelper.GetValidationMessage(validationResult));
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
