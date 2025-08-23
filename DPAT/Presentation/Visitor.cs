using System;
using System.Collections.Generic;
using System.Linq;
using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Presentation
{
    public class Visitor : IVisitor
    {
        private readonly Dictionary<string, (int x, int y)> _statePositions = new();
        private readonly List<string> _renderedElements = new();
        private readonly FSM _fsm;
        private readonly Dictionary<string, string> _triggerMap;
        private readonly Dictionary<string, IState> _stateMap;
        private readonly List<string> _outputLines = new();

        public Visitor(FSM fsm)
        {
            _fsm = fsm;
            _triggerMap = _fsm.Triggers.ToDictionary(t => t.Identifier, t => t.Description);
            _stateMap = _fsm.States.ToDictionary(s => s.Identifier, s => s);
        }

        public IReadOnlyList<string> OutputLines => _outputLines.AsReadOnly();

        // Visitor pattern implementation - these methods handle the double dispatch
        public void Visit(IState state)
        {
            // This should not be called directly - use Accept() on the state instead
            throw new InvalidOperationException("Use Accept() method on state objects instead of calling Visit directly");
        }

        public void Visit(InitialState state)
        {
            ProcessInitialState(state);
        }

        public void Visit(FinalState state)
        {
            ProcessFinalState(state);
        }

        public void Visit(CompoundState state)
        {
            ProcessCompoundState(state);
        }

        public void Visit(SimpleState state)
        {
            ProcessSimpleState(state);
        }

        public void Visit(Transition transition)
        {
            ProcessTransitionSummary(transition);
        }

        public void Visit(Trigger trigger)
        {
            // Triggers are processed as part of transitions
            _outputLines.Add($"Trigger: {trigger.Description}");
        }

        public void Visit(Action action)
        {
            // Actions are processed as part of transitions or states
            _outputLines.Add($"Action: {action.Description} ({action.Type})");
        }

        private void ProcessInitialState(InitialState state)
        {
            _outputLines.Add($"O Initial state ({state.Name})");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);

            var initTransitions = _fsm.Transitions.Where(t => t.Connection.Item1.Identifier == state.Identifier);
            foreach (var t in initTransitions)
            {
                _outputLines.Add("");
                _outputLines.Add($"---{GetTransitionText(t)}---> {_stateMap[t.Connection.Item2.Identifier].Name}");
            }
        }

        private void ProcessFinalState(FinalState state)
        {
            _outputLines.Add("");
            _outputLines.Add($"(O) Final state ({state.Name})");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void ProcessCompoundState(CompoundState state)
        {
            // Add compound state header with double equals
            _outputLines.Add("");
            _outputLines.Add($"{new string('=', 70)}");
            _outputLines.Add($"|| Compound state: {state.Name}");
            _outputLines.Add($"{new string('=', 70)}");
            
            // Process compound contents with proper indentation (start with depth 1 for sub-states)
            ProcessCompoundContents(state, 1);
            
            // Add compound state footer
            _outputLines.Add("");
            _outputLines.Add($"{new string('=', 70)}");

            var outgoing = _fsm.Transitions.Where(t => t.Connection.Item1.Identifier == state.Identifier);
            foreach (var t in outgoing)
            {
                var to = _stateMap[t.Connection.Item2.Identifier];
                var toLabel = to is FinalState ? $"Final state ({to.Name})" : to.Name;
                _outputLines.Add($"---{GetTransitionText(t)}---> {toLabel}");
            }

            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void ProcessSimpleState(SimpleState state)
        {
            _outputLines.Add($"{state.Name}");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void ProcessTransitionSummary(Transition transition)
        {
            var fromState = transition.Connection.Item1;
            var toState = transition.Connection.Item2;

            var triggerLabel = !string.IsNullOrEmpty(transition.Trigger) && _triggerMap.TryGetValue(transition.Trigger!, out var desc)
                ? desc
                : (transition.Trigger ?? "");
            var guardLabel = !string.IsNullOrEmpty(transition.Guard) ? $"[{transition.Guard}]" : "";
            var effect = GetEffectDescription(transition);
            var effectSuffix = string.IsNullOrEmpty(effect) ? string.Empty : $" / {effect}";
            var labelPieces = new[] { triggerLabel, guardLabel }.Where(p => !string.IsNullOrEmpty(p));
            var label = string.Join(' ', labelPieces);
            var fullLabel = ($"{label}{effectSuffix}").Trim();

            _outputLines.Add($"# {fromState.Name} --{fullLabel}--> {toState.Name}");
            _renderedElements.Add(transition.Identifier);
        }

        private string GetEffectDescription(Transition transition)
        {
            if (!string.IsNullOrEmpty(transition.EffectActionIdentifier))
            {
                var a = _fsm.Actions.FirstOrDefault(a => a.Identifier == transition.EffectActionIdentifier && a.Type == ActionType.TRANSITION_ACTION);
                if (a != null) return a.Description;
                return transition.EffectActionIdentifier!;
            }

            var effect = _fsm.Actions.FirstOrDefault(a => a.Identifier == transition.Identifier && a.Type == ActionType.TRANSITION_ACTION);
            return effect?.Description ?? string.Empty;
        }

        private string GetTransitionText(Transition transition)
        {
            var trigger = !string.IsNullOrEmpty(transition.Trigger) && _triggerMap.TryGetValue(transition.Trigger!, out var desc)
                ? desc
                : (transition.Trigger ?? "");
            var guard = !string.IsNullOrEmpty(transition.Guard) ? $"[{transition.Guard}]" : "";
            var effect = GetEffectDescription(transition);
            var effectSuffix = string.IsNullOrEmpty(effect) ? string.Empty : $" / {effect}";
            var pieces = new[] { trigger, guard }.Where(p => !string.IsNullOrEmpty(p));
            var label = string.Join(' ', pieces);
            return $"{label}{effectSuffix}".Trim();
        }

        private void ProcessCompoundContents(CompoundState compound, int depth)
        {
            var indent = new string(' ', depth * 4); // Use 4 spaces for indentation
            var subStates = compound.SubStates.ToList();
            foreach (var sub in subStates)
            {
                if (sub is CompoundState nested)
                {
                    _outputLines.Add("");
                    _outputLines.Add($"{indent}{new string('=', 70)}");
                    _outputLines.Add($"{indent}|| Compound state: {nested.Name}");
                    _outputLines.Add($"{indent}{new string('=', 70)}");
                    ProcessCompoundContents(nested, depth + 1);
                    _outputLines.Add("");
                    _outputLines.Add($"{indent}{new string('=', 70)}");
                    _outputLines.Add("");
                }
                else
                {
                    _outputLines.Add("");
                    _outputLines.Add($"{indent}{new string('-', 70)}");
                    _outputLines.Add($"{indent}| {sub.Name}");
                    _outputLines.Add($"{indent}{new string('-', 70)}");
                    foreach (var actionLine in GetStateActionsLines(sub))
                    {
                        _outputLines.Add($"{indent}| {actionLine}");
                    }
                    _outputLines.Add($"{indent}{new string('-', 70)}");

                    var internalTransitions = _fsm.Transitions.Where(t =>
                             t.Connection.Item1.Identifier == sub.Identifier &&
                             subStates.Any(s => s.Identifier == t.Connection.Item2.Identifier));
                    foreach (var t in internalTransitions)
                    {
                        _outputLines.Add($"{indent}---{GetTransitionText(t)}---> {_stateMap[t.Connection.Item2.Identifier].Name}");
                    }
                }
            }
        }

        private IEnumerable<string> GetStateActionsLines(IState state)
        {
            var actions = _fsm.Actions.Where(a => a.Identifier == state.Identifier).ToList();
            var lines = new List<string>();
            foreach (var a in actions.Where(a => a.Type == ActionType.ENTRY_ACTION))
            {
                lines.Add($"On Entry / {a.Description}");
            }
            foreach (var a in actions.Where(a => a.Type == ActionType.DO_ACTION))
            {
                lines.Add($"On Do / {a.Description}");
            }
            foreach (var a in actions.Where(a => a.Type == ActionType.EXIT_ACTION))
            {
                lines.Add($"On Exit / {a.Description}");
            }
            return lines;
        }
    }
}

