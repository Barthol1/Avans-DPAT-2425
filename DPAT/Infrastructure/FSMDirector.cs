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

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
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

            var idToState = parsedStates.ToDictionary(s => s.Identifier, s => s);
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine) || line.StartsWith('#')) continue;
                if (trimmedLine.StartsWith("STATE"))
                {
                    var match = Regex.Match(trimmedLine, "^STATE\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*|_)\\s+\"([^\"]*)\"\\s*:\\s*(INITIAL|SIMPLE|COMPOUND|FINAL)\\s*;$");
                    if (!match.Success) continue;
                    var stateId = match.Groups[1].Value;
                    var parentId = match.Groups[2].Value;
                    if (parentId != "_" && idToState.TryGetValue(stateId, out var child) && idToState.TryGetValue(parentId, out var parent) && parent is CompoundState compound)
                    {
                        compound.SubStates.Add(child);
                    }
                }
            }

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
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