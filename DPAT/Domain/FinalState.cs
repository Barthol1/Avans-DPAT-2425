using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class FinalState : IState
    {
        public string Name { get; set; }
        public string Identifier { get; set; }

        public FinalState(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}