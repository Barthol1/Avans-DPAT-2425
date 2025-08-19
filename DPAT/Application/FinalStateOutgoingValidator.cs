using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class FinalStateOutgoingValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            foreach (var transition in fsm.Transitions)
            {
                if (transition.Connection.Item1 is FinalState finalState)
                {
                    throw new InvalidOperationException($"Final state '{finalState.Name}' cannot have outgoing transitions.");
                }
            }
        }
    }
}