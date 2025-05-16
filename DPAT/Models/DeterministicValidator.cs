namespace DPAT.Models
{
    public class DeterministicValidator : IFSMValidator
    {
        public bool Validate(FSM fsm)
        {
            return true;
        }
    }
}