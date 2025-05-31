using System.Text.RegularExpressions;
using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Infrastructure
{
    public class FSMParser
    {
        public IState GetState(string line)
        {
            var stateRegex = $"^STATE\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*|_)\\s+\"([^\"]*)\"\\s*:\\s*(INITIAL|SIMPLE|COMPOUND|FINAL)\\s*;$";
            var match = Regex.Match(line, stateRegex);
            if (!match.Success)
            {
                throw new Exception("Invalid state line");
            }

            var identifier = match.Groups[1].Value;
            var parentId = match.Groups[2].Value;
            var name = match.Groups[3].Value;
            var type = match.Groups[4].Value;

            IState state = type switch
            {
                "INITIAL" => new InitialState(identifier, name),
                "SIMPLE" => new SimpleState(identifier, name),
                "COMPOUND" => new CompoundState(identifier, name),
                "FINAL" => new FinalState(identifier, name),
                _ => throw new Exception("Invalid state type")
            };

            return state;
        }

        public Transition GetTransition(string line)
        {
            var transitionRegexPattern = $"^TRANSITION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s*->\\s*([a-zA-Z][a-zA-Z0-9_]*)" +
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +       // Optional trigger_identifier (Group 4)
                                    $"\\s*\"([^\"]*)\"" +                      // Guard condition (Group 5) - always quotes, content can be empty
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +       // Optional effect_action_identifier (Group 6)
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

            // Note: In a real implementation, you would need to find the actual state objects
            // This is simplified for the example
            var source = new SimpleState(sourceId, "");
            var destination = new SimpleState(destinationId, "");
            var transition  = new Transition
            {
                Connection = new Tuple<IState, IState>(source, destination),
                Identifier = match.Groups[1].Value,
                Trigger = string.IsNullOrEmpty(triggerId) ? null : triggerId,
                Guard = string.IsNullOrEmpty(guard) ? null : guard
            };

            return transition;
        }

        public Action GetAction(string line)
        {
            var actionRegex = $"^ACTION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+\"([^\"]*)\"\\s*:\\s*(ENTRY_ACTION|DO_ACTION|EXIT_ACTION|TRANSITION_ACTION)\\s*;$";
            var match = Regex.Match(line, actionRegex);
            if (!match.Success)
            {
                throw new Exception("Invalid action line");
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
                _ => throw new Exception("Invalid action type")
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
                throw new Exception("Invalid trigger line");
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