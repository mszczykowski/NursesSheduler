namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface IBuilder<T> where T : class
    {
        T GetResult();
    }
}
