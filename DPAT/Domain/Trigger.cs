using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Trigger(string identifier, string description) : IFSMComponent
    {
        public required string Identifier { get; set; } = identifier;
        public required string Description { get; set; } = description;

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