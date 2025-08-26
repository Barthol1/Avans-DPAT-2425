using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class State : IFSMComponent
    {
        public string Name { get; set; }

        public State(string name)
        {
            Identifier = identifier;
            Name = name;
        }

        public void print()
        {
            //TODO implement print logic
        }

        public void validate()
        {
            //TODO implement validation logic
        }
    }
}
