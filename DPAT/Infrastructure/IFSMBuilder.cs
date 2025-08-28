using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public interface IFSMBuilder
    {
        public void AddState(string identifier, string name, StateType type);
        public void AddAction(string identifier, string description, ActionType type);
        public void AddTrigger(string identifier, string description);
        public void AddTransition(string sourceState, string targetState, string? triggerIdentifier, string? guard, string? effectActionIdentifier);
        FSM Build();
        void Reset();
    }
}