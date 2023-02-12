namespace NursesScheduler.Domain.Models.Calendar
{
    public sealed class Holiday
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string CountryCode { get; set; }
    }
}
