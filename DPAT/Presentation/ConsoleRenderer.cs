using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Presentation
{
    public class ConsoleRenderer : IRenderer
    {
        public void Render(FSM fsm)
        {
            RenderAsciiDiagram(fsm);
        }

        private void RenderAsciiDiagram(FSM fsm)
        {
            var stateMap = fsm.States.ToDictionary(s => s.Identifier, s => s);
            var triggerMap = fsm.Triggers.ToDictionary(t => t.Identifier, t => t.Description);

            var initial = fsm.States.OfType<InitialState>().FirstOrDefault();
            var final = fsm.States.OfType<FinalState>().FirstOrDefault();

            Console.WriteLine("FSM Visualization:");
            Console.WriteLine("####################################################################");
            Console.WriteLine("# Diagram: FSM");
            Console.WriteLine("####################################################################");
            Console.WriteLine();

            if (initial != null)
            {
                Console.WriteLine($"O Initial state ({initial.Name})");
                var initTransitions = fsm.Transitions.Where(t => t.Connection.Item1.Identifier == initial.Identifier);
                foreach (var t in initTransitions)
                {
                    Console.WriteLine();
                    Console.WriteLine($"---{GetTransitionText(t, fsm, triggerMap)}---> {stateMap[t.Connection.Item2.Identifier].Name}");
                }
            }

            var compoundStates = fsm.States.OfType<CompoundState>().ToList();
            var childIds = new HashSet<string>(compoundStates.SelectMany(c => c.SubStates.Select(s => s.Identifier)));
            foreach (var compound in compoundStates)
            {
                if (childIds.Contains(compound.Identifier)) continue;
                Console.WriteLine();
                Console.WriteLine(new string('=', 74));
                Console.WriteLine($"|| Compound state: {compound.Name}");
                Console.WriteLine(new string('-', 74));

                RenderCompoundContents(fsm, compound, stateMap, triggerMap, 1);

                Console.WriteLine();
                var outgoing = fsm.Transitions.Where(t => t.Connection.Item1.Identifier == compound.Identifier);
                foreach (var t in outgoing)
                {
                    var to = stateMap[t.Connection.Item2.Identifier];
                    var toLabel = to is FinalState ? $"Final state ({to.Name})" : to.Name;
                    Console.WriteLine($"---{GetTransitionText(t, fsm, triggerMap)}---> {toLabel}");
                }

                Console.WriteLine();
                Console.WriteLine(new string('=', 74));
            }

            if (final != null)
            {
                Console.WriteLine();
                Console.WriteLine($"O Final state ({final.Name})");
            }

            Console.WriteLine();
            Console.WriteLine("Transition Summary:");
            Console.WriteLine("####################################################################");
            foreach (var transition in fsm.Transitions)
            {
                var fromState = stateMap[transition.Connection.Item1.Identifier];
                var toState = stateMap[transition.Connection.Item2.Identifier];
                var trigger = !string.IsNullOrEmpty(transition.Trigger) && triggerMap.TryGetValue(transition.Trigger!, out var desc)
                    ? desc
                    : (transition.Trigger ?? "");
                var guard = !string.IsNullOrEmpty(transition.Guard) ? $"[{transition.Guard}]" : "";
                var effect = GetEffectDescription(transition, fsm);
                var effectSuffix = string.IsNullOrEmpty(effect) ? string.Empty : $" / {effect}";
                var pieces = new[] { trigger, guard }.Where(p => !string.IsNullOrEmpty(p));
                var label = string.Join(' ', pieces);
                var fullLabel = $"{label}{effectSuffix}".Trim();

                Console.WriteLine($"# {fromState.Name} --{fullLabel}--> {toState.Name}");
            }
            Console.WriteLine("####################################################################");
        }

        private void RenderCompoundContents(
            FSM fsm,
            CompoundState compound,
            Dictionary<string, IState> stateMap,
            Dictionary<string, string> triggerMap,
            int depth)
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
                    RenderCompoundContents(fsm, nested, stateMap, triggerMap, depth + 1);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"{tab}{new string('-', 70)}");
                    Console.WriteLine($"{tab}| {sub.Name}");
                    Console.WriteLine($"{tab}{new string('-', 70)}");
                    foreach (var actionLine in GetStateActionsLines(sub, fsm))
                    {
                        Console.WriteLine($"{tab}| {actionLine}");
                    }
                    Console.WriteLine($"{tab}{new string('-', 70)}");

                    var internalTransitions = fsm.Transitions.Where(t =>
                             t.Connection.Item1.Identifier == sub.Identifier &&
                             subStates.Any(s => s.Identifier == t.Connection.Item2.Identifier));
                    foreach (var t in internalTransitions)
                    {
                        Console.WriteLine($"{tab}---{GetTransitionText(t, fsm, triggerMap)}---> {stateMap[t.Connection.Item2.Identifier].Name}");
                    }
                }
            }
        }

        private string GetTransitionText(Transition transition, FSM fsm, Dictionary<string, string> triggerMap)
        {
            var trigger = !string.IsNullOrEmpty(transition.Trigger) && triggerMap.TryGetValue(transition.Trigger!, out var desc)
                ? desc
                : (transition.Trigger ?? "");
            var guard = !string.IsNullOrEmpty(transition.Guard) ? $"[{transition.Guard}]" : "";
            var effect = GetEffectDescription(transition, fsm);
            var effectSuffix = string.IsNullOrEmpty(effect) ? string.Empty : $" / {effect}";
            var pieces = new[] { trigger, guard }.Where(p => !string.IsNullOrEmpty(p));
            var label = string.Join(' ', pieces);
            return $"{label}{effectSuffix}".Trim();
        }

        private string GetEffectDescription(Transition transition, FSM fsm)
        {
            if (!string.IsNullOrEmpty(transition.EffectActionIdentifier))
            {
                var a = fsm.Actions.FirstOrDefault(a => a.Identifier == transition.EffectActionIdentifier && a.Type == ActionType.TRANSITION_ACTION);
                if (a != null) return a.Description;
                return transition.EffectActionIdentifier!;
            }

            var effect = fsm.Actions.FirstOrDefault(a => a.Identifier == transition.Identifier && a.Type == ActionType.TRANSITION_ACTION);
            return effect?.Description ?? string.Empty;
        }

        private IEnumerable<string> GetStateActionsLines(IState state, FSM fsm)
        {
            var actions = fsm.Actions.Where(a => a.Identifier == state.Identifier).ToList();
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