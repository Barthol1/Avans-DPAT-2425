using System.Text.RegularExpressions;
using DPAT.Domain;

namespace DPAT.Infrastructure
{
    public record ParsedState(string Identifier, string? Parent, string Name, StateType Type);
    public record ParsedTransition(string Identifier, string SourceId, string TargetId, string? TriggerName, string GuardCondition);
    public record ParsedAction(string Identifier, string Description, ActionType Type);
    public record ParsedTrigger(string Identifier, string Description);

    public class FSMParser
    {
        private static readonly Regex StateRegex = new($"^STATE\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*|_)\\s+\"([^\"]*)\"\\s*:\\s*(INITIAL|SIMPLE|COMPOUND|FINAL)\\s*;$", RegexOptions.Compiled);
        private static readonly Regex TransitionRegex = new($"^TRANSITION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s*->\\s*([a-zA-Z][a-zA-Z0-9_]*)" +
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +
                                    $"(?:\\s*\"([^\"]*)\")?" +
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +
                                    $"\\s*;$", RegexOptions.Compiled);
        private static readonly Regex ActionRegex = new("^ACTION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*:\\s*(ENTRY_ACTION|DO_ACTION|EXIT_ACTION|TRANSITION_ACTION)\\s*;$", RegexOptions.Compiled);
        private static readonly Regex TriggerRegex = new("^TRIGGER\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*;$", RegexOptions.Compiled);

        public ParsedState ParseState(string line)
        {
            var match = StateRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid state line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var parent = match.Groups[2].Value;
            var name = match.Groups[3].Value;
            var type = match.Groups[4].Value;

            var stateType = type switch
            {
                "INITIAL" => StateType.INITIAL,
                "SIMPLE" => StateType.SIMPLE,
                "COMPOUND" => StateType.COMPOUND,
                "FINAL" => StateType.FINAL,
                _ => throw new ArgumentException($"Invalid state type: {type}")
            };

            return new ParsedState(identifier, parent == "_" ? null : parent, name, stateType);
        }

        public ParsedTransition ParseTransition(string line)
        {
            var match = TransitionRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid transition line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var sourceId = match.Groups[2].Value;
            var targetId = match.Groups[3].Value;
            var triggerName = match.Groups[4].Value;
            var guardCondition = match.Groups[5].Value;

            return new ParsedTransition(
                identifier,
                sourceId,
                targetId,
                string.IsNullOrEmpty(triggerName) ? null : triggerName,
                guardCondition
            );
        }

        public ParsedAction ParseAction(string line)
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
                _ => throw new ArgumentException($"Invalid action type: {type}")
            };

            return new ParsedAction(identifier, description, actionType);
        }

        public ParsedTrigger ParseTrigger(string line)
        {
            var match = TriggerRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid trigger line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var description = match.Groups[2].Value;

            return new ParsedTrigger(identifier, description);
        }
    }
}