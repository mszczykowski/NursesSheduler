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
    internal sealed class RegisterViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private bool _isFormActive;
        public bool IsFormActive
        {
            get => _isFormActive;
            set
            {
                _isFormActive = value;
                OnPropertyChanged(nameof(IsFormActive));
            }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                IsFormActive = !value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
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
        private string _passwordRepeated;
        public string PasswordRepeated 
        {
            get => _passwordRepeated; 
            set
            {
                _passwordRepeated = value;
                OnPropertyChanged(nameof(PasswordRepeated));
            }
        }

        public ICommand CreatePasswordCommand { get; }

        public ICommand ExitCommand { get; }

        public RegisterViewModel(IDatabaseService databaseManager, NavigationService<LogInViewModel> logInViewNavigationService)
        {
            IsFormActive = true;

            ExitCommand = new ExitCommand();

            CreatePasswordCommand = new CreateDbCommand(this, databaseManager, logInViewNavigationService);

            _errorsViewModel = new ErrorsViewModel();

            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
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
            ValidatePasswordRepeat();
        }

        private void ValidatePassword()
        {
            _errorsViewModel.ClearErrors(nameof(Password));

            var validationResult = PasswordValidator.ValidatePassword(Password);

            if (validationResult != PasswordValidationResult.Valid)
            {
                _errorsViewModel.AddError(nameof(Password),
                    PasswordValidationMessageHelper.GetValidationMessage(validationResult));
            }
        }

        private void ValidatePasswordRepeat()
        {
            _errorsViewModel.ClearErrors(nameof(PasswordRepeated));

            if (Password != PasswordRepeated)
                _errorsViewModel.AddError(nameof(PasswordRepeated), PasswordValidationMessageHelper.GetNotMatchingPasswordMessage());
        }
    }
}
