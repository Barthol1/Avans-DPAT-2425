using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class InitialIngoingValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            var initialState = fsm.States.Where(s => s is InitialState).FirstOrDefault();
            if (initialState == null)
            {
                throw new Exception("FSM must have exactly one initial state.");
            }
            foreach (var transition in fsm.Transitions)
            {
                if (transition.Connection.Item2 is InitialState)
                {
                    throw new InvalidOperationException($"Initial state '{initialState.Name}' cannot have incoming transitions from '{transition.Connection.Item1.Name}'.");
                }
            }
        }
    }
}