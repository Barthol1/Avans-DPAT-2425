using System.Text.RegularExpressions;
using DPAT.Domain;

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

        public (string identifier, string name, StateType type) ParseState(string line)
        {
            var match = StateRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid state line: {line}");
            }

            var identifier = match.Groups[1].Value;
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

            return (identifier, name, stateType);
        }

        public (string sourceState, string targetState, string? triggerIdentifier, string? guard, string? effectActionIdentifier) ParseTransition(string line)
        {
            var match = TransitionRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid transition line: {line}");
            }

            var sourceId = match.Groups[2].Value;
            var destinationId = match.Groups[3].Value;
            var triggerId = match.Groups[4].Value;
            var guard = match.Groups[5].Value;
            var effectActionId = match.Groups[6].Value;

            return (
                sourceId,
                destinationId,
                string.IsNullOrEmpty(triggerId) ? null : triggerId,
                string.IsNullOrEmpty(guard) ? null : guard,
                string.IsNullOrEmpty(effectActionId) ? null : effectActionId
            );
        }

        public (string identifier, string description, ActionType type) ParseAction(string line)
        {
            var match = ActionRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid action line: {line}");
            }

            var identifier = match.Groups[1].Value;
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

            return (identifier, description, actionType);
        }

        public (string identifier, string description) ParseTrigger(string line)
        {
            var match = TriggerRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid trigger line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var description = match.Groups[2].Value;

            return (identifier, description);
        }
    }
}