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

        public Visitor(FSM fsm)
        {
            _fsm = fsm;
            _triggerMap = _fsm.Triggers.ToDictionary(t => t.Identifier, t => t.Description);
            _stateMap = _fsm.States.ToDictionary(s => s.Identifier, s => s);
        }

        public void Visit(IState state)
        {
            switch (state)
            {
                case InitialState initial:
                    RenderInitialState(initial);
                    break;
                case FinalState final:
                    RenderFinalState(final);
                    break;
                case CompoundState compound:
                    RenderCompoundState(compound);
                    break;
                case SimpleState simple:
                    RenderSimpleState(simple);
                    break;
                default:
                    RenderGenericState(state);
                    break;
            }
        }

        public void Visit(Transition transition)
        {
            RenderTransitionSummary(transition);
        }

        public void Visit(Trigger trigger)
        {
            // Triggers are rendered as part of transitions
        }

        public void Visit(Action action)
        {
            // Actions are rendered as part of transitions or states
        }

        private void RenderInitialState(InitialState state)
        {
            Console.WriteLine($"O Initial state ({state.Name})");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);

            var initTransitions = _fsm.Transitions.Where(t => t.Connection.Item1.Identifier == state.Identifier);
            foreach (var t in initTransitions)
            {
                Console.WriteLine();
                Console.WriteLine($"---{GetTransitionText(t)}---> {_stateMap[t.Connection.Item2.Identifier].Name}");
            }
        }

        private void RenderFinalState(FinalState state)
        {
            Console.WriteLine($"O Final state ({state.Name})");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderCompoundState(CompoundState state)
        {
            // Do not print outer separators or compound header; only the indented contents and outgoing transitions
            RenderCompoundContents(state, 1);
            Console.WriteLine();

            var outgoing = _fsm.Transitions.Where(t => t.Connection.Item1.Identifier == state.Identifier);
            foreach (var t in outgoing)
            {
                var to = _stateMap[t.Connection.Item2.Identifier];
                var toLabel = to is FinalState ? $"Final state ({to.Name})" : to.Name;
                Console.WriteLine($"---{GetTransitionText(t)}---> {toLabel}");
            }

            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderSimpleState(SimpleState state)
        {
            Console.WriteLine($"{state.Name}");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderGenericState(IState state)
        {
            Console.WriteLine($"{state.Name}");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderTransitionSummary(Transition transition)
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

            Console.WriteLine($"# {fromState.Name} --{fullLabel}--> {toState.Name}");
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

        private void RenderCompoundContents(CompoundState compound, int depth)
        {
            var tab = new string('\t', depth);
            var subStates = compound.SubStates.ToList();
            foreach (var sub in subStates)
            {
                if (sub is CompoundState nested)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{tab}{new string('-', 70)}");
                    Console.WriteLine($"{tab}| Compound state: {nested.Name}");
                    Console.WriteLine($"{tab}{new string('-', 70)}");
                    RenderCompoundContents(nested, depth + 1);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"{tab}{new string('-', 70)}");
                    Console.WriteLine($"{tab}| {sub.Name}");
                    Console.WriteLine($"{tab}{new string('-', 70)}");
                    foreach (var actionLine in GetStateActionsLines(sub))
                    {
                        Console.WriteLine($"{tab}| {actionLine}");
                    }
                    Console.WriteLine($"{tab}{new string('-', 70)}");

                    var internalTransitions = _fsm.Transitions.Where(t =>
                             t.Connection.Item1.Identifier == sub.Identifier &&
                             subStates.Any(s => s.Identifier == t.Connection.Item2.Identifier));
                    foreach (var t in internalTransitions)
                    {
                        Console.WriteLine($"{tab}---{GetTransitionText(t)}---> {_stateMap[t.Connection.Item2.Identifier].Name}");
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
