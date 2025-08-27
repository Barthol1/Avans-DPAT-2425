using DPAT.Domain;
using Action = DPAT.Domain.Action;

namespace DPAT.Domain.Interfaces
{
    public interface IVisitor
    {
        void Visit(State state);
        void Visit(Transition transition);
        void Visit(Trigger trigger);
        void Visit(Action action);
    }
}