using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    class FinalState : IState
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public List<IState> Outgoing { get; set; } = [];
        public List<IState> Incoming { get; set; } = [];

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