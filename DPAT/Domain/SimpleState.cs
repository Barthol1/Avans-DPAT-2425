using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    class SimpleState : IState
    {
        public string Name { get; set; }
        public string Identifier { get; set; }

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
