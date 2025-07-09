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
                    if (!((CompoundState)targetState).SubStates.Any(s => s is SimpleState))
                    {
                        throw new Exception($"Transition {transition.Identifier} targets a compound state without simple states: {targetState.Identifier}");
                    }
                }
            }
        }
    }
}