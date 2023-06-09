namespace NursesScheduler.WPF.Services.Interfaces
{
    internal interface IPasswordService
    {
        void TryRemovePassword();
        string? TryRetrievePassword();
        void SavePassword(string password);
    }
}