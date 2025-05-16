namespace DPAT.Models
{
    public class UnreachableStateValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            return true;
        }
    }
}