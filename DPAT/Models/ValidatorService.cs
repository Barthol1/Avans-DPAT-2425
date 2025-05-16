namespace DPAT.Models
{
    public class ValidatorService
    {
        private List<IFSMValidator> _validators = new List<IFSMValidator>();

        public void AddValidator(IFSMValidator validator)
        {
            _validators.Add(validator);
        }

        public bool Validate(FSM fsm)
        {
            foreach (var validator in _validators)
            {
                if (!validator.Validate(fsm))
                {
                    return false;
                }
            }

            return true;
        }
    }
}