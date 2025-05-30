using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public interface IFSMBuilder
    {
        void AddState(IState state);
        void AddAction(Action action);
        void AddTrigger(Trigger trigger);
        void AddTransition(IState source, IState destination, string? triggerId = null, string? guard = null, string? effectActionId = null);
        FSM Build();
        void Reset();
    }
}