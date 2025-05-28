using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class FinalStateOutgoingValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            var finalStates = fsm.States.Where(s => s is FinalState).ToList();
            foreach (var finalState in finalStates)
            {
                if (finalState.Outgoing.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}