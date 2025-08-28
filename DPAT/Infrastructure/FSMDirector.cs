using System.Runtime.CompilerServices;
using DPAT.Domain;
using DPAT.Domain.Interfaces;


namespace DPAT.Infrastructure
{
    public class FSMDirector
    {
        private IFSMBuilder _builder;
        private readonly FSMParser _parser;

        public FSMDirector(IFSMBuilder builder, FSMParser? parser = null)
        {
            _builder = builder;
            _parser = parser ?? new FSMParser();
        }

        public void ChangeBuilder(IFSMBuilder builder)
        {
            _builder = builder;
        }

        public IFSMComponent Make(List<string> lines)
        {
            _builder.Reset();

            // First pass: Parse and add all states, actions, and triggers
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                    continue;

                var componentType = line.Split(' ')[0] switch
                {
                    "STATE" => ComponentType.STATE,
                    "TRANSITION" => ComponentType.TRANSITION,
                    "ACTION" => ComponentType.ACTION,
                    "TRIGGER" => ComponentType.TRIGGER,
                    _ => throw new FormatException($"Invalid line: {line}")
                };

                if (componentType == ComponentType.STATE)
                {
                    var parsedState = _parser.ParseState(line);
                    _builder.AddState(parsedState);
                }
                else if (componentType == ComponentType.ACTION)
                {
                    var parsedAction = _parser.ParseAction(line);
                    _builder.AddAction(parsedAction);
                }
                else if (componentType == ComponentType.TRIGGER)
                {
                    var parsedTrigger = _parser.ParseTrigger(line);
                    _builder.AddTrigger(parsedTrigger);
                }
            }

            // Second pass: Add transitions (requires states to be created first)
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                    continue;

                if (line.StartsWith("TRANSITION"))
                {
                    var parsedTransition = _parser.ParseTransition(line);
                    _builder.AddTransition(parsedTransition);
                }
            }

            return _builder.Build();
        }
    }
}