using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public interface IFSMBuilder
    {
        public void AddState(State state);
        public void AddAction(Action action);
        public void AddTrigger(Trigger trigger);
        public void AddTransition(Transition transition);
        FSM Build();
        void Reset();
    }
}