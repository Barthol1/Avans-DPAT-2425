using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class State : IFSMComponent
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public StateType Type { get; set; }

        public State(string identifier, string name, StateType type)
        {
            Identifier = identifier;
            Name = name;
            Type = type;
        }
        public void Print()
        {
            Console.WriteLine($"State: {Identifier} - {Name} - {Type}");
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
