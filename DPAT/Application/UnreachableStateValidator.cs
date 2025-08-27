using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Application
{
    public class UnreachableStateValidator : IFSMValidator
    {
        public void Validate(IFSMComponent fsm)
        {
            throw new NotImplementedException();
        }


    }
}