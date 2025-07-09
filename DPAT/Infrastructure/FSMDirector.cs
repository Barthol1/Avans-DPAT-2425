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
            var parsedStates = new List<IState>();

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
                    var state = _parser.GetState(trimmedLine);
                    parsedStates.Add(state);
                    _builder.AddState(state);
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

            // Second pass: parse transitions with access to the parsed states
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || line.StartsWith('#'))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("TRANSITION"))
                {
                    _builder.AddTransition(_parser.GetTransition(trimmedLine, parsedStates));
                }
            }

            var fsm = _builder.Build();
            return fsm;
        }
    }
}