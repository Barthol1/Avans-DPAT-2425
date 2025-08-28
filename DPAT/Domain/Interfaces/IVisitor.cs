using DPAT.Domain;
using Action = DPAT.Domain.Action;

namespace DPAT.Domain.Interfaces
{
    public interface IVisitor
    {
        void Print(State state);
        void Print(Transition transition);
        void Print(Trigger trigger);
        void Print(Action action);
    }
}