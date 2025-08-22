using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class CompoundState : IState
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public List<IState> SubStates { get; set; }

        public CompoundState(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
            SubStates = new List<IState>();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}