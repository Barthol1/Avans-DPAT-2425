using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Presentation
{
    public class DrawConsoleVisitor : IVisitor
    {
        private const int BoxWidth = 65;
        private const int StateNameWidth = 20;
        private const int StateTypeWidth = 15;
        private const int TransitionNameWidth = 15;
        private const int TriggerWidth = 20;
        private const int GuardWidth = 15;
        private const int ActionDescriptionWidth = 20;
        private const int ActionTypeWidth = 15;

        private readonly List<string> _outputLines = new();
        public IEnumerable<string> OutputLines => _outputLines.AsReadOnly();

        public void Print(State state)
        {
            _outputLines.Add("┌-----------------------------------------------------------------");
            _outputLines.Add($"| State: {state.Name,-StateNameWidth} | Type: {state.Type,-StateTypeWidth}");
            _outputLines.Add("└-----------------------------------------------------------------");
        }

        public void Print(Transition transition)
        {
            _outputLines.Add("┌-----------------------------------------------------------------");
            _outputLines.Add($"| Transition: {transition.SourceState.Name,-TransitionNameWidth} → {transition.TargetState.Name,-TransitionNameWidth}");
            _outputLines.Add($"| Trigger: {transition.Trigger,-TriggerWidth} | Guard: {transition.Guard,-GuardWidth}");
            _outputLines.Add("└-----------------------------------------------------------------");
        }

        public void Print(Trigger trigger)
        {
            _outputLines.Add("┌-----------------------------------------------------------------");
            _outputLines.Add($"| Trigger: {trigger.Description,-BoxWidth}");
            _outputLines.Add("└-----------------------------------------------------------------");
        }

        public void Print(Action action)
        {
            _outputLines.Add("┌-----------------------------------------------------------------");
            _outputLines.Add($"| Action: {action.Description,-ActionDescriptionWidth} | Type: {action.Type,-ActionTypeWidth}");
            _outputLines.Add("└-----------------------------------------------------------------");
        }
    }
}

