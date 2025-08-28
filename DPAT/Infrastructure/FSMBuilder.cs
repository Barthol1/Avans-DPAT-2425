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

        public void AddAction(string description, ActionType type = ActionType.TRANSITION_ACTION)
        {
            var action = new Action
            {
                Description = description,
                Type = type
            };
            _fsm.Add(action);
        }

        public void AddState(string name, StateType type)
        {
            var state = new State(name, type)
            {
                Name = name,
                Type = type
            };
            _fsm.Add(state);
        }

        public void AddTransition(State sourceState, State targetState, string? guard, Action? effectAction)
        {
            var transition = new Transition
            {
                SourceState = sourceState,
                TargetState = targetState,
                Guard = string.IsNullOrEmpty(guard) ? null : guard,
                Action = effectAction
            };
            _fsm.Add(transition);
        }

        public void AddTrigger(string description)
        {
            var trigger = new Trigger
            {
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