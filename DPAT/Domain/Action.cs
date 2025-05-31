using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Action: IIdentifier,IDrawable
    {
        public required string Identifier { get; set; }
        public required string Description { get; set; }
        public required ActionType Type { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}