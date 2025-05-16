using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class InitialIngoingValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            return true;
        }
    }
}