using NursesScheduler.WPF.Commands;
using NursesScheduler.WPF.Commands.Common;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.Services.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace NursesScheduler.WPF.ViewModels
{
    internal class ChangePasswordViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                ValidateForm();
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
                ValidateForm();
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
                ValidatePasswordRepeat();
                OnPropertyChanged(nameof(NewPasswordRepeated));
            }
        }

        public ICommand ChangePasswordCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly IDatabaseService _databaseService;
        private readonly IPasswordService _passwordService;

        public ChangePasswordViewModel(IDatabaseService databaseManager, NavigationService<LogInViewModel> logInViewNavigationService)
        {
            CancelCommand = new NavigateCommand<LogInViewModel>(logInViewNavigationService);

            ChangePasswordCommand = new ChangeDbPasswordCommand();

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
            ValidateNewPassword();
            ValidatePasswordRepeat();
        }

        private void ValidateNewPassword()
        {
            _errorsViewModel.ClearErrors(nameof(NewPassword));

            if (string.IsNullOrEmpty(NewPassword)) _errorsViewModel.AddError(nameof(NewPassword), "Password can't be empty");
        }

        private void ValidatePasswordRepeat()
        {
            _errorsViewModel.ClearErrors(nameof(NewPasswordRepeated));

            if (NewPassword != NewPasswordRepeated) _errorsViewModel.AddError(nameof(NewPasswordRepeated), "Password doesn't match");
        }
    }
}
