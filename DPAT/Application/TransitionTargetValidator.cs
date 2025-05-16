using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class TransitionTargetValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            return true;
        }
    }
}