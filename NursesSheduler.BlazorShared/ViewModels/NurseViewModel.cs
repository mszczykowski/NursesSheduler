namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class NurseViewModel
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsDeleted { get; set; }
        public override string ToString()
        {
            return Name + Surname;
        }
    }
}
