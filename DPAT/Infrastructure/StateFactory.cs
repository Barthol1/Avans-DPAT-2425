using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Infrastructure
{
    public class StateFactory : IFSMComponentFactory
    {
        public IFSMComponent Create(string type, string identifier, string name)
        {
            return type switch
            {
                "INITIAL" => new State(identifier, name, StateType.INITIAL),
                "SIMPLE" => new State(identifier, name, StateType.SIMPLE),
                "COMPOUND" => new State(identifier, name, StateType.COMPOUND),
                "FINAL" => new State(identifier, name, StateType.FINAL),
                _ => throw new Exception($"Invalid state type: {type}")
            };
        }
    }
}



