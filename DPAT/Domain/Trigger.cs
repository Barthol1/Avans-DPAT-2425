using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Trigger: IFSMComponent
    {
        public required string Identifier { get; set; }
        public required string Description { get; set; }
        
        public void Print()
        {
            throw new NotImplementedException();
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}