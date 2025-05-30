using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class ValidatorService
    {
        private readonly List<IFSMValidator> _validators = [];

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