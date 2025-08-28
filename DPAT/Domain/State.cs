using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class State : IFSMComponent
    {
        public string Name { get; set; } = string.Empty;
        public StateType Type { get; set; } = StateType.SIMPLE;

        public void Print(IVisitor visitor)
        {
            visitor.Print(this);
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
