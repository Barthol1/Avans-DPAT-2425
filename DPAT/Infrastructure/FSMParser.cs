using System.Text.RegularExpressions;
using DPAT.Domain;
using System;
using Action = DPAT.Domain.Action;
using IState = DPAT.Domain.Interfaces.IState;

namespace DPAT.Infrastructure
{
    public class FSMParser
    {
        private readonly List<IState> _states = [];
        private readonly List<Action> _actions = [];
        private readonly List<Trigger> _triggers = [];

        public FSM ParseFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine) || line.StartsWith('#'))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("STATE"))
                {
                    ParseState(trimmedLine);
                }
                else if (trimmedLine.StartsWith("TRANSITION"))
                {
                    ParseTransition(trimmedLine);
                }
                else if (trimmedLine.StartsWith("ACTION"))
                {
                    ParseAction(trimmedLine);
                }
                else if (trimmedLine.StartsWith("TRIGGER"))
                {
                    ParseTrigger(trimmedLine);
                }
            }

            return new FSM
            {
                States = _states,
                Actions = _actions,
                Triggers = _triggers
            };
        }

        private void ParseState(string line)
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

            IState? parent = parentId == "_" ? null : _states.FirstOrDefault(s => s.Identifier == parentId);


            IState state = type switch
            {
                "INITIAL" => new InitialState(identifier, name),
                "SIMPLE" => new SimpleState(identifier, name),
                "COMPOUND" => new CompoundState(identifier, name),
                "FINAL" => new FinalState(identifier, name),
                _ => throw new Exception("Invalid state type")
            };

            state.Parent = parent;

            if (parent is CompoundState compound)
            {
                compound.SubStates.Add(state);
            }

            _states.Add(state);
        }

        private void ParseTransition(string line)
        {
            var transitionRegexPattern = $"^TRANSITION\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s+([a-zA-Z][a-zA-Z0-9_]*)\\s*->\\s*([a-zA-Z][a-zA-Z0-9_]*)" +
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +       // Optionele trigger_identifier (Groep 4)
                                    $"\\s*\"([^\"]*)\"" +                      // Guard condition (Groep 5) - altijd quotes, inhoud kan leeg
                                    $"(?:\\s+([a-zA-Z][a-zA-Z0-9_]*))?" +       // Optionele effect_action_identifier (Groep 6)
                                    $"\\s*;$";
            var match = Regex.Match(line, transitionRegexPattern);
            if (!match.Success)
            {
                throw new Exception("Invalid transition line: " + line);
            }

            var transitionId = match.Groups[1].Value;
            var sourceId = match.Groups[2].Value;
            var destinationId = match.Groups[3].Value;
            var triggerId = match.Groups[4].Value;
            var guard = match.Groups[5].Value;

            var source = _states.Find(s => s.Identifier == sourceId);
            var destination = _states.Find(s => s.Identifier == destinationId);

            if (source == null || destination == null)
            {
                throw new Exception("States required for transition not found");
            }

            source.Outgoing.Add(destination);
            destination.Incoming.Add(source);

        }

        private void ParseAction(string line)
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
                Id = actionId,
                Description = actionDescription,
                Type = actionTypeEnum
            };

            _actions.Add(action);
        }

        private void ParseTrigger(string line)
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
                Id = triggerId,
                Description = triggerDescription
            };

            _triggers.Add(trigger);
        }
    }
}