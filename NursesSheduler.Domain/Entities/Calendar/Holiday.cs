namespace NursesScheduler.Domain.Entities.Calendar
{
    public sealed record Holiday(DateTime Date, string LocalName, string Name, string CountryCode);
}
