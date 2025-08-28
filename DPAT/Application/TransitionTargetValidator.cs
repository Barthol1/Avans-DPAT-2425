using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class TransitionTargetValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            foreach (var transition in fsm.Components.OfType<Transition>())
            {
                if (transition.TargetState.Type == StateType.COMPOUND)
                {
                    throw new InvalidOperationException($"Transition {transition.SourceState.Name} -> {transition.TargetState.Name} targets a compound state '{transition.TargetState.Name}' instead of one of its children.");
                }
            }
        }
    }
}