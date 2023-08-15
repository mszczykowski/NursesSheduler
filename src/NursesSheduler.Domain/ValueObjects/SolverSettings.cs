namespace NursesScheduler.Domain.ValueObjects
{
    public sealed class SolverSettings
    {
        public int NumberOfRetries { get; set; }
        public bool UseOwnSeed { get; set; }
        public string GeneratorSeed { get; set; }
    }
}
