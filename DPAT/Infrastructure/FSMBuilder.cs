using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public class FSMBuilder : IFSMBuilder
    {
        private FSM _fsm;
        private readonly Dictionary<string, State> _states = new();
        private readonly Dictionary<string, Action> _actions = new();
        private readonly Dictionary<string, Trigger> _triggers = new();

        public FSMBuilder()
        {
            _fsm = new FSM();
        }



        public void AddAction(ParsedAction parsedAction)
        {
            var action = new Action
            {
                Description = parsedAction.Description,
                Type = parsedAction.Type
            };
            _actions[parsedAction.Identifier] = action;
            _fsm.Add(action);
        }



        public void AddState(ParsedState parsedState)
        {
            var state = new State
            {
                Name = parsedState.Name,
                Type = parsedState.Type
            };
            _states[parsedState.Identifier] = state;
            _fsm.Add(state);
        }



        public void AddTransition(ParsedTransition parsedTransition)
        {
            if (!_states.TryGetValue(parsedTransition.SourceId, out var sourceState))
                throw new InvalidOperationException($"Source state '{parsedTransition.SourceId}' not found");

            if (!_states.TryGetValue(parsedTransition.TargetId, out var targetState))
                throw new InvalidOperationException($"Target state '{parsedTransition.TargetId}' not found");

            var transition = new Transition
            {
                SourceState = sourceState,
                TargetState = targetState,
                Guard = parsedTransition.GuardCondition
            };

            if (parsedTransition.TriggerName != null && _triggers.TryGetValue(parsedTransition.TriggerName, out var trigger))
            {
                transition.Trigger = parsedTransition.TriggerName;
            }

            _fsm.Add(transition);
        }



        public void AddTrigger(ParsedTrigger parsedTrigger)
        {
            var trigger = new Trigger
            {
                Description = parsedTrigger.Description
            };
            _triggers[parsedTrigger.Identifier] = trigger;
            _fsm.Add(trigger);
        }

        public FSM Build()
        {
            return _fsm;
        }

        public void Reset()
        {
            _fsm = new FSM();
            _states.Clear();
            _actions.Clear();
            _triggers.Clear();
        }
    }
}