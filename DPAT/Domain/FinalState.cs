using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    class FinalState : IState
    {
        public string Name { get; set; }
        public IState? Parent { get; set; }
        public string Identifier { get; set; }

        public FinalState(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }
    }
}