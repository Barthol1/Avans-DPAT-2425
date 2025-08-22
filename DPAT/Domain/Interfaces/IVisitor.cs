using DPAT.Domain;
using Action = DPAT.Domain.Action;

namespace DPAT.Domain.Interfaces
{
    public interface IVisitor
    {
        void Visit(IState state);
        void Visit(InitialState state);
        void Visit(FinalState state);
        void Visit(CompoundState state);
        void Visit(SimpleState state);
        void Visit(Transition transition);
        void Visit(Trigger trigger);
        void Visit(Action action);
    }
}