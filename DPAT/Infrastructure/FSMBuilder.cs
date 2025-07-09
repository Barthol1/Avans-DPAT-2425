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
            this._fsm = new FSM()
            {
                Actions = new List<Action>(),
                States = new List<IState>(),
                Transitions = new List<Transition>(),
                Triggers = new List<Trigger>()
            };
        }

        public void AddAction(Action action)
        {
            this._fsm.Actions.Add(action);
        }

        public void AddState(IState state)
        {
            this._fsm.States.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            this._fsm.Transitions.Add(transition);
        }

        public void AddTrigger(Trigger trigger)
        {
            this._fsm.Triggers.Add(trigger);
        }

        public FSM Build()
        {
            return this._fsm;
        }

        public void Reset()
        {
            this._fsm = new FSM()
            {
                Actions = new List<Action>(),
                States = new List<IState>(),
                Transitions = new List<Transition>(),
                Triggers = new List<Trigger>()
            };
        }
    }
}