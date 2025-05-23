using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    class InitialState : IState
    {
        public string Name { get; set; }
        public IState? Parent { get; set; }
        public string Identifier { get; set; }

        public InitialState(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }
    }
}
