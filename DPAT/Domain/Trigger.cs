using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Trigger : IFSMComponent
    {
        public required string Description { get; set; }

        public void Print(IVisitor visitor)
        {
            visitor.Print(this);
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}