using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Infrastructure
{
    public class SimpleStateFactory : IStateFactory
    {
        public IState Create(string type, string identifier, string name)
        {
            return type switch
            {
                "INITIAL" => new InitialState(identifier, name),
                "SIMPLE" => new SimpleState(identifier, name),
                "COMPOUND" => new CompoundState(identifier, name),
                "FINAL" => new FinalState(identifier, name),
                _ => throw new Exception("Invalid state type")
            };
        }
    }
}



