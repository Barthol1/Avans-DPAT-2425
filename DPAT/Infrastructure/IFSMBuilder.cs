using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public interface IFSMBuilder
    {
        public void AddState(ParsedState parsedState);
        public void AddAction(ParsedAction parsedAction);
        public void AddTrigger(ParsedTrigger parsedTrigger);
        public void AddTransition(ParsedTransition parsedTransition);
        FSM Build();
        void Reset();
    }
}