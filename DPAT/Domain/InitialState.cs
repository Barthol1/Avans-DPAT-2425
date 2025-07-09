using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    class InitialState : IState
    {
        public string Name { get; set; }
        public string Identifier { get; set; }

        public InitialState(string identifier, string name)
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
