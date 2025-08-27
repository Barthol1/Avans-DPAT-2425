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

        public State GetState(string line)
        {
            var match = StateRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid state line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var name = match.Groups[3].Value;
            var type = match.Groups[4].Value;

            var stateFactory = new StateFactory();
            var state = stateFactory.Create(type, identifier, name);

            return (State)state;
        }

        public Transition GetTransition(string line)
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

            var transition = new Transition
            {
                SourceState = sourceId,
                TargetState = destinationId,
                Trigger = string.IsNullOrEmpty(triggerId) ? null : triggerId,
                EffectActionIdentifier = string.IsNullOrEmpty(effectActionId) ? null : effectActionId,
                Guard = string.IsNullOrEmpty(guard) ? null : guard
            };

            return transition;
        }

        public Action GetAction(string line)
        {
            var match = ActionRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid action line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var description = match.Groups[2].Value;
            var type = match.Groups[3].Value;

            var action = new Action
            {
                Identifier = identifier,
                Description = description,
                Type = type switch
                {
                    "ENTRY_ACTION" => ActionType.ENTRY_ACTION,
                    "DO_ACTION" => ActionType.DO_ACTION,
                    "EXIT_ACTION" => ActionType.EXIT_ACTION,
                    "TRANSITION_ACTION" => ActionType.TRANSITION_ACTION,
                    _ => throw new NotImplementedException($"Invalid action type: {type}")
                }
            };

            return action;
        }

        public Trigger GetTrigger(string line)
        {
            var match = TriggerRegex.Match(line);
            if (!match.Success)
            {
                throw new FormatException($"Invalid trigger line: {line}");
            }

            var identifier = match.Groups[1].Value;
            var description = match.Groups[2].Value;

            return new Trigger(identifier, description)
            {
                Identifier = identifier,
                Description = description
            };
        }
    }
}