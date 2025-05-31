namespace DPAT.Domain.Interfaces
{
    public interface IVisitor
    {
        void Visit(IState state);
        void Visit(Transition transition);
        void Visit(Trigger trigger);
        void Visit(Action action);
    }
}