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

        public void AddAction(Action action)
        {
            _fsm.Add(action);
        }

        public void AddState(State state)
        {
            _fsm.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            _fsm.Add(transition);
        }

        public void AddTrigger(Trigger trigger)
        {
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