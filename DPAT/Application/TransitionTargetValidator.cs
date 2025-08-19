using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class TransitionTargetValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            foreach (var transition in fsm.Transitions)
            {
                var targetState = transition.Connection.Item2;
                if (targetState is CompoundState)
                {
                    throw new InvalidOperationException($"Transition {transition.Identifier} targets a compound state '{targetState.Identifier}' instead of one of its children.");
                }
            }
        }
    }
}