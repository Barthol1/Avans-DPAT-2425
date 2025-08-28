using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Presentation
{
    public class DrawConsoleVisitor : IVisitor
    {
        private readonly List<string> _outputLines = new();
        public IEnumerable<string> OutputLines => _outputLines.AsReadOnly();

        public void Print(State state)
        {
            _outputLines.Add("┌─────────────────────────────────────────────────────────┐");
            _outputLines.Add($"│ State: {state.Name,-20} │ Type: {state.Type,-15} │");
            _outputLines.Add("└─────────────────────────────────────────────────────────┘");
        }

        public void Print(Transition transition)
        {
            _outputLines.Add("┌─────────────────────────────────────────────────────────┐");
            _outputLines.Add($"│ Transition: {transition.SourceState.Name,-15} → {transition.TargetState.Name,-15} │");
            _outputLines.Add($"│ Trigger: {transition.Trigger,-20} │ Guard: {transition.Guard,-15} │");
            _outputLines.Add("└─────────────────────────────────────────────────────────┘");
        }

        public void Print(Trigger trigger)
        {
            _outputLines.Add("┌─────────────────────────────────────────────────────────┐");
            _outputLines.Add($"│ Trigger: {trigger.Description,-45} │");
            _outputLines.Add("└─────────────────────────────────────────────────────────┘");
        }

        public void Print(Action action)
        {
            _outputLines.Add("┌─────────────────────────────────────────────────────────┐");
            _outputLines.Add($"│ Action: {action.Description,-20} │ Type: {action.Type,-15} │");
            _outputLines.Add("└─────────────────────────────────────────────────────────┘");
        }
    }
}

