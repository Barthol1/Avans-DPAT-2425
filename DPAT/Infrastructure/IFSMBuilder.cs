using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public interface IFSMBuilder
    {
        public void AddState(string name, StateType type);
        public void AddAction(string description, ActionType type = ActionType.TRANSITION_ACTION);
        public void AddTrigger(string description);
        public void AddTransition(State sourceState, State targetState, string? guard, Action? action);
        FSM Build();
        void Reset();
    }
}