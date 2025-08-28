using System.Text.RegularExpressions;
using DPAT.Domain;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public class FSMParser
    {
        private static readonly Regex StateRegex = new($"^STATE\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*|_)\\s+\"([^\"]*)\"\\s*:\\s*(INITIAL|SIMPLE|COMPOUND|FINAL)\\s*;$", RegexOptions.Compiled);
        private static readonly Regex TransitionRegex = new("^TRANSITION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s*->\\s*([a-zA-Z][a-zA-Z0-9_]*)" +
                                        $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +
                                        $"\\s*\"([^\"]*)\"" +
                                        $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +
                                        $"\\s*;$", RegexOptions.Compiled);
        private static readonly Regex ActionRegex = new("^ACTION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*:\\s*(ENTRY_ACTION|DO_ACTION|EXIT_ACTION|TRANSITION_ACTION)\\s*;$", RegexOptions.Compiled);
        private static readonly Regex TriggerRegex = new("^TRIGGER\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*;$", RegexOptions.Compiled);

        public (string name, StateType type) ParseState(string line)
        {
            var match = StateRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid state line: {line}");
            }

            var name = match.Groups[3].Value;
            var type = match.Groups[4].Value;

            var stateType = type switch
            {
                "INITIAL" => StateType.INITIAL,
                "SIMPLE" => StateType.SIMPLE,
                "COMPOUND" => StateType.COMPOUND,
                "FINAL" => StateType.FINAL,
                _ => throw new NotImplementedException($"Invalid state type: {type}")
            };

            return (name, stateType);
        }

        public (State sourceState, State targetState, string? guard, Action? action) ParseTransition(string line)
        {
            var sourceState = new State();
            var targetState = new State();
            var action = new Action();

            var match = TransitionRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid transition line: {line}");
            }

            var sourceName = match.Groups[1].Value;
            var targetName = match.Groups[2].Value;
            var guard = match.Groups[4].Value;
            var effectActionName = match.Groups[5].Value;

            return (
                sourceState,
                new State() { Name = targetName },
                string.IsNullOrEmpty(guard) ? null : guard,
                string.IsNullOrEmpty(effectActionName) ? null : new Action() { Name = effectActionName }
            );
        }

        public (string description, ActionType type) ParseAction(string line)
        {
            var match = ActionRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid action line: {line}");
            }

            var description = match.Groups[2].Value;
            var type = match.Groups[3].Value;

            var actionType = type switch
            {
                "ENTRY_ACTION" => ActionType.ENTRY_ACTION,
                "DO_ACTION" => ActionType.DO_ACTION,
                "EXIT_ACTION" => ActionType.EXIT_ACTION,
                "TRANSITION_ACTION" => ActionType.TRANSITION_ACTION,
                _ => throw new NotImplementedException($"Invalid action type: {type}")
            };

            return (description, actionType);
        }

        public string ParseTrigger(string line)
        {
            var match = TriggerRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid trigger line: {line}");
            }

            var description = match.Groups[2].Value;

            return description;
        }
    }
}