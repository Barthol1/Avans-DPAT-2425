using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Action : IFSMComponent
    {
        public required string Description { get; set; }
        public required ActionType Type { get; set; }

        public void Print(IVisitor visitor)
        {
            visitor.Print(this);
        }
    }
}