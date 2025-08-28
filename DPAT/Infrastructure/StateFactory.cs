using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Infrastructure
{
    public class StateFactory : IFSMComponentFactory
    {
        public IFSMComponent Create(string type, string name)
        {
            return type switch
            {
                "INITIAL" => new State() { Name = name, Type = StateType.INITIAL },
                "SIMPLE" => new State() { Name = name, Type = StateType.SIMPLE },
                "COMPOUND" => new State() { Name = name, Type = StateType.COMPOUND },
                "FINAL" => new State() { Name = name, Type = StateType.FINAL },
                _ => throw new Exception($"Invalid state type: {type}")
            };
        }
    }
}



