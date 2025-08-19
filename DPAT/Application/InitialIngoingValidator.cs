using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class InitialIngoingValidator : IFSMValidator
    {
        public void Validate(FSM fsm)
        {
            var initialStates = fsm.States.OfType<InitialState>().ToList();
            if (initialStates.Count != 1)
            {
                throw new Exception("FSM must have exactly one initial state.");
            }
            var initialState = initialStates.Single();
            foreach (var transition in fsm.Transitions)
            {
                if (transition.Connection.Item2 is InitialState)
                {
                    throw new InvalidOperationException($"Initial state '{initialState.Name}' cannot have incoming transitions.");
                }
            }
        }
    }
}