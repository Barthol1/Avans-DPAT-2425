using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class TransitionTargetValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            // check if there are any transitions that target a compound state instead of a simple state inside the compound state
            foreach (var state in fsm.States)
            {
                if (state.Outgoing.Any(o => o is CompoundState))
                {
                    return false;
                }
            }
            return true;
        }
    }
}