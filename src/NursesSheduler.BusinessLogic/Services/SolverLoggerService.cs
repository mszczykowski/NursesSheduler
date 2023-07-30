using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SolverLoggerService : ISolverLoggerService
    {
        public IReadOnlyCollection<SolverLog> SolverLogs => _solverLogs;

        private event Action LogUpdated;

        private readonly ICurrentDateService _currentDateService;

        private List<SolverLog> _solverLogs;
        private int _currentSolverRun;

        public SolverLoggerService(ICurrentDateService currentDateService)
        {
            _currentDateService = currentDateService;

            _solverLogs = new List<SolverLog>();
        }

        public void InitialiseLogger(Action subscriberMethod)
        {
            _solverLogs.Clear();
            _currentSolverRun = 0;

            Subscribe(subscriberMethod);
        }

        public void LogEvent(SolverEvents solverEvent)
        {
            CreateLog(solverEvent, null);
        }

        public void LogEvent(SolverEvents solverEvent, SolverAbortReasons abortReason)
        {
            CreateLog(solverEvent, abortReason);
        }

        private void CreateLog(SolverEvents solverEvent, SolverAbortReasons? abortReason)
        {
            if (solverEvent == SolverEvents.Started)
            {
                _currentSolverRun++;
            }

            _solverLogs.Add(new SolverLog
            {
                LogDate = _currentDateService.GetCurrentDateTime(),
                SolverEvent = solverEvent,
                AbortReason = abortReason,
                CurrentSolverRun = _currentSolverRun,
            });

            PublishLogUpdated();

            if (solverEvent == SolverEvents.Finished)
            {
                UnsubscribeAll();
            }
        }

        private void UnsubscribeAll()
        {
            if (LogUpdated is null)
            {
                return;
            }

            foreach (Action a in LogUpdated.GetInvocationList())
            {
                LogUpdated -= a;
            }
        }

        private void Subscribe(Action subscriberMethod)
        {
            LogUpdated += subscriberMethod;
        }

        private void PublishLogUpdated()
        {
            LogUpdated?.Invoke();
        }
    }
}
