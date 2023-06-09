using AdysTech.CredentialManager;
using NursesScheduler.WPF.Services.Interfaces;
using System.Configuration;
using System.Net;

namespace NursesScheduler.WPF.Services.Implementation
{
    internal sealed class PasswordService : IPasswordService
    {
        private readonly string appName;
        public PasswordService()
        {
            appName = ConfigurationManager.AppSettings["appName"];
        }
        public void SavePassword(string password)
        {
            var credentials = new NetworkCredential(appName, password);
            CredentialManager.SaveCredentials(appName, credentials);
        }

        public void TryRemovePassword()
        {
            if(CredentialManager.GetCredentials(appName) != null)
                CredentialManager.RemoveCredentials(appName);
        }

        public string? TryRetrievePassword()
        {
            var credentials = CredentialManager.GetCredentials(appName);

            if (credentials == null) 
                return null;

            return credentials.Password;
        }
    }
}
