using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public class FSMBuilder : IFSMBuilder
    {
        private readonly List<IState> _states = [];
        private readonly List<Action> _actions = [];
        private readonly List<Trigger> _triggers = [];

        public void AddState(IState state)
        {
            _states.Add(state);
        }

        public void AddAction(Action action)
        {
            _actions.Add(action);
        }

        public void AddTrigger(Trigger trigger)
        {
            _triggers.Add(trigger);
        }

        public void AddTransition(IState source, IState destination, string? triggerId = null, string? guard = null, string? effectActionId = null)
        {
            source.Outgoing.Add(destination);
            destination.Incoming.Add(source);
        }

        public FSM Build()
        {
            return new FSM
            {
                States = _states,
                Actions = _actions,
                Triggers = _triggers
            };
        }

        public void Reset()
        {
            _states.Clear();
            _actions.Clear();
            _triggers.Clear();
        }
    }
}