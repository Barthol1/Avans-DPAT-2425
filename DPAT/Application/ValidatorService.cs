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

        public void Validate(IFSMComponent fsm)
        {
            foreach (var validator in _validators)
            {
                validator.Validate(fsm);
            }
        }
    }
}