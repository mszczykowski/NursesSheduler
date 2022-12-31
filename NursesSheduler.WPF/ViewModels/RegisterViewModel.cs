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
    internal class RegisterViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _password;
        public String Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                ValidateForm();
                OnPropertyChanged(nameof(Password));
            }
        }
        private string _passwordRepeated;
        public String PasswordRepeated 
        {
            get => _passwordRepeated; 
            set
            {
                _passwordRepeated = value;
                ValidatePasswordRepeat();
                OnPropertyChanged(nameof(PasswordRepeated));
            }
        }

        public ICommand CreatePasswordCommand { get; }

        public ICommand ExitCommand { get; }

        public RegisterViewModel(IDatabaseService databaseManager, NavigationService<LogInViewModel> logInViewNavigationService)
        {
            ExitCommand = new ExitCommand();

            CreatePasswordCommand = new CreatePasswordCommand(this, databaseManager, logInViewNavigationService);

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

            if (string.IsNullOrEmpty(Password)) _errorsViewModel.AddError(nameof(Password), "Password can't be empty");
        }

        private void ValidatePasswordRepeat()
        {
            _errorsViewModel.ClearErrors(nameof(PasswordRepeated));

            if (Password != PasswordRepeated) _errorsViewModel.AddError(nameof(PasswordRepeated), "Password doesn't match");
        }
    }
}
