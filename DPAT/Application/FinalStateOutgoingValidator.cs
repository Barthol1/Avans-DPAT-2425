using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class FinalStateOutgoingValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            foreach (var transition in fsm.Components.OfType<Transition>())
            {
                if (transition.SourceState.Type == StateType.FINAL)
                {
                    throw new InvalidOperationException($"Final state '{transition.SourceState.Name}' cannot have outgoing transitions.");
                }
            }
        }
    }
}