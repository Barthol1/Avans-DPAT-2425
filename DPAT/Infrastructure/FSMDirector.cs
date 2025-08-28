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
                    var (identifier, name, type) = _parser.ParseState(line);
                    _builder.AddState(identifier, name, type);
                }
                else if (componentType == ComponentType.TRANSITION)
                {
                    var (sourceState, targetState, triggerIdentifier, guard, effectActionIdentifier) = _parser.ParseTransition(line);
                    _builder.AddTransition(sourceState, targetState, triggerIdentifier, guard, effectActionIdentifier);
                }
                else if (componentType == ComponentType.ACTION)
                {
                    var (identifier, description, type) = _parser.ParseAction(line);
                    _builder.AddAction(identifier, description, type);
                }
                else if (componentType == ComponentType.TRIGGER)
                {
                    var (identifier, description) = _parser.ParseTrigger(line);
                    _builder.AddTrigger(identifier, description);
                }
                else
                    throw new FormatException($"Invalid line: {line}");
            }
            return _builder.Build();
        }
    }
}