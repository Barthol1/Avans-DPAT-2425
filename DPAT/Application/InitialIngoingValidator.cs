using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class InitialIngoingValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            var initialState = fsm.States.Where(s => s is InitialState).FirstOrDefault();
            if (initialState == null)
            {
                return false;
            }
            if (initialState.Incoming.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}