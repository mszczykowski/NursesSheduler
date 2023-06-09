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
    internal sealed class ChangePasswordViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private string _newPasswordRepeated;
        public string NewPasswordRepeated
        {
            get => _newPasswordRepeated;
            set
            {
                _newPasswordRepeated = value;
                OnPropertyChanged(nameof(NewPasswordRepeated));
            }
        }

        public ICommand ChangePasswordCommand { get; }
        public ICommand BackCommand { get; }

        private readonly IDatabaseService _databaseService;

        public ChangePasswordViewModel(IDatabaseService databaseService, IPasswordService passwordService,
            NavigationService<LogInViewModel> logInViewNavigationService)
        {
            _databaseService = databaseService;

            BackCommand = new NavigateCommand<LogInViewModel>(logInViewNavigationService);

            ChangePasswordCommand = new ChangeDbPasswordCommand(this, databaseService, passwordService, logInViewNavigationService);

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
            ValidateOldPassword();
            ValidateNewPassword();
            ValidatePasswordRepeat();
        }

        private async void ValidateOldPassword()
        {
            _errorsViewModel.ClearErrors(nameof(OldPassword));

            if (string.IsNullOrEmpty(OldPassword))
                _errorsViewModel.AddError(nameof(OldPassword), 
                    PasswordValidationMessageHelper.GetValidationMessage(PasswordValidationResult.Empty));
            else if (await _databaseService.TryGetConnectionStingFromPassword(OldPassword) == null)
                _errorsViewModel.AddError(nameof(OldPassword), PasswordValidationMessageHelper.GetWrongPasswordMessage());
        }

        private void ValidateNewPassword()
        {
            _errorsViewModel.ClearErrors(nameof(NewPassword));

            var validationResult = PasswordValidator.ValidatePassword(NewPassword);

            if(validationResult != PasswordValidationResult.Valid)
                _errorsViewModel.AddError(nameof(NewPassword),
                        PasswordValidationMessageHelper.GetValidationMessage(validationResult));
        }

        private void ValidatePasswordRepeat()
        {
            _errorsViewModel.ClearErrors(nameof(NewPasswordRepeated));

            if (NewPassword != NewPasswordRepeated) 
                _errorsViewModel.AddError(nameof(NewPasswordRepeated), PasswordValidationMessageHelper.GetNotMatchingPasswordMessage());
        }
    }
}
