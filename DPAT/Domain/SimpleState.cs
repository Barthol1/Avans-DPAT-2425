using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    class SimpleState : IState
    {
        public string Name { get; set; }
        public IState? Parent { get; set; }
        public string Identifier { get; set; }
        public List<IState> Outgoing { get; set; } = [];
        public List<IState> Incoming { get; set; } = [];

        public SimpleState(string identifier, string name)
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
