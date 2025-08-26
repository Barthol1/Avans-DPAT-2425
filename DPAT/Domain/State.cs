using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class State : IFSMComponent
    {
        public string Name { get; set; }

        public State(string name)
        {
            Name = name;
        }

        public void Print()
        {
            throw new NotImplementedException();
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
