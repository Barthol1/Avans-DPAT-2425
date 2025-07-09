using System.Text.RegularExpressions;
using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    class FSMDirector
    {
        private IFSMBuilder _builder;
        private FSMParser _parser = new FSMParser();

        public FSMDirector(IFSMBuilder builder)
        {
            this._builder = builder;
        }

        public void ChangeBuilder(IFSMBuilder builder)
        {
            this._builder = builder;
        }

        public FSM Build()
        {
            return this._builder.Build();
        }

        public FSM BuildFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var tempTransitions = new List<Transition>();

            // First pass: collect all states
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || line.StartsWith('#'))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("STATE"))
                {
                    _builder.AddState(_parser.GetState(trimmedLine));
                }
                else if (trimmedLine.StartsWith("TRANSITION"))
                {
                    tempTransitions.Add(_parser.GetTransition(trimmedLine));
                }
                else if (trimmedLine.StartsWith("ACTION"))
                {
                    _builder.AddAction(_parser.GetAction(trimmedLine));
                }
                else if (trimmedLine.StartsWith("TRIGGER"))
                {
                    _builder.AddTrigger(_parser.GetTrigger(trimmedLine));
                }
            }

            // Second pass: link transitions to actual state objects
            var fsm = _builder.Build();
            var stateMap = fsm.States.ToDictionary(s => s.Identifier, s => s);

            foreach (var tempTransition in tempTransitions)
            {
                var sourceState = stateMap.GetValueOrDefault(tempTransition.Connection.Item1.Identifier);
                var targetState = stateMap.GetValueOrDefault(tempTransition.Connection.Item2.Identifier);

                if (sourceState != null && targetState != null)
                {
                    var linkedTransition = new Transition
                    {
                        Identifier = tempTransition.Identifier,
                        Connection = new Tuple<IState, IState>(sourceState, targetState),
                        Trigger = tempTransition.Trigger,
                        Guard = tempTransition.Guard
                    };

                    fsm.Transitions.Add(linkedTransition);
                }
            }

            return fsm;
        }
    }
}