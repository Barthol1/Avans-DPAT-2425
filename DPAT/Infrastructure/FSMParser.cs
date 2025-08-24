using System.Text.RegularExpressions;
using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public class FSMParser
    {
        private readonly IStateFactory _stateFactory;

        public FSMParser(IStateFactory? stateFactory = null)
        {
            _stateFactory = stateFactory ?? new SimpleStateFactory();
        }

        public IState GetState(string line)
        {
            var stateRegex = $"^STATE\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*|_)\\s+\"([^\"]*)\"\\s*:\\s*(INITIAL|SIMPLE|COMPOUND|FINAL)\\s*;$";
            var match = Regex.Match(line, stateRegex);
            if (!match.Success)
            {
                throw new Exception("Invalid state line");
            }

            var identifier = match.Groups[1].Value;
            var name = match.Groups[3].Value;
            var type = match.Groups[4].Value;

            IState state = _stateFactory.Create(type, identifier, name);

            return state;
        }

        public Transition GetTransition(string line, IEnumerable<IState> states)
        {
            var transitionRegexPattern = $"^TRANSITION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s*->\\s*([a-zA-Z][a-zA-Z0-9_]*)" +
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +
                                    $"(?:\\s*\"([^\"]*)\")?" +
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +
                                    $"\\s*;$";
            var match = Regex.Match(line, transitionRegexPattern);
            if (!match.Success)
            {
                throw new Exception("Invalid transition line: " + line);
            }

            var sourceId = match.Groups[2].Value;
            var destinationId = match.Groups[3].Value;
            var triggerId = match.Groups[4].Value;
            var guard = match.Groups[5].Value;
            var effectActionId = match.Groups[6].Value;

            var source = states.FirstOrDefault(s => s.Identifier == sourceId);
            var destination = states.FirstOrDefault(s => s.Identifier == destinationId);

            if (source == null)
            {
                throw new Exception($"Source state '{sourceId}' not found");
            }
            if (destination == null)
            {
                throw new Exception($"Destination state '{destinationId}' not found");
            }

            var transition = new Transition
            {
                Connection = new Tuple<IState, IState>(source, destination),
                Identifier = match.Groups[1].Value,
                Trigger = string.IsNullOrEmpty(triggerId) ? null : triggerId,
                Guard = string.IsNullOrEmpty(guard) ? null : guard,
                EffectActionIdentifier = string.IsNullOrEmpty(effectActionId) ? null : effectActionId
            };

            return transition;
        }

        public Action GetAction(string line)
        {
            var actionRegex = $"^ACTION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*:\\s*(ENTRY_ACTION|DO_ACTION|EXIT_ACTION|TRANSITION_ACTION)\\s*;$";
            var match = Regex.Match(line, actionRegex);
            if (!match.Success)
            {
                throw new NotSupportedException("Invalid action line");
            }

            var actionId = match.Groups[1].Value;
            var actionDescription = match.Groups[2].Value;
            var actionType = match.Groups[3].Value;

            ActionType actionTypeEnum = actionType switch
            {
                "ENTRY_ACTION" => ActionType.ENTRY_ACTION,
                "DO_ACTION" => ActionType.DO_ACTION,
                "EXIT_ACTION" => ActionType.EXIT_ACTION,
                "TRANSITION_ACTION" => ActionType.TRANSITION_ACTION,
                _ => throw new NotSupportedException("Invalid action type")
            };

            Action action = new()
            {
                Identifier = actionId,
                Description = actionDescription,
                Type = actionTypeEnum
            };

            return action;
        }

        public Trigger GetTrigger(string line)
        {
            var triggerRegex = $"^TRIGGER\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*;$";
            var match = Regex.Match(line, triggerRegex);
            if (!match.Success)
            {
                throw new NotSupportedException("Invalid trigger line");
            }

            var triggerId = match.Groups[1].Value;
            var triggerDescription = match.Groups[2].Value;

            Trigger trigger = new()
            {
                Identifier = triggerId,
                Description = triggerDescription
            };

            return trigger;
        }
    }
}