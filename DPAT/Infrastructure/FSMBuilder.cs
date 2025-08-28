using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public class FSMBuilder : IFSMBuilder
    {
        private FSM _fsm;

        public FSMBuilder()
        {
            _fsm = new FSM();
        }

        public void AddAction(string identifier, string description, ActionType type)
        {
            var action = new Action
            {
                Identifier = identifier,
                Description = description,
                Type = type
            };
            _fsm.Add(action);
        }

        public void AddState(string identifier, string name, StateType type)
        {
            var state = new State(identifier, name, type);
            _fsm.Add(state);
        }

        public void AddTransition(string sourceState, string targetState, string? triggerIdentifier, string? guard, string? effectActionIdentifier)
        {
            var transition = new Transition
            {
                SourceState = sourceState,
                TargetState = targetState,
                Trigger = string.IsNullOrEmpty(triggerIdentifier) ? null : triggerIdentifier,
                Guard = string.IsNullOrEmpty(guard) ? null : guard,
                EffectActionIdentifier = string.IsNullOrEmpty(effectActionIdentifier) ? null : effectActionIdentifier
            };
            _fsm.Add(transition);
        }

        public void AddTrigger(string identifier, string description)
        {
            var trigger = new Trigger(identifier, description)
            {
                Identifier = identifier,
                Description = description
            };
            _fsm.Add(trigger);
        }

        public FSM Build()
        {
            return _fsm;
        }

        public void Reset()
        {
            _fsm = new FSM();
        }
    }
}