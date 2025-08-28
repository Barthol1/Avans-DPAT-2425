using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class State : IFSMComponent
    {
        public string Name { get; set; } = string.Empty;
        public StateType Type { get; set; } = StateType.SIMPLE;
        public List<Action> Actions { get; } = new();

        public void Print(IVisitor visitor)
        {
            visitor.Print(this);
        }
    }
}
