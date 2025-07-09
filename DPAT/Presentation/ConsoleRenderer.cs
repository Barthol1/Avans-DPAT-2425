using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Presentation
{
    public class ConsoleRenderer : IRenderer
    {
        public void Render(FSM fsm)
        {
            Console.WriteLine("FSM Visualization:");
            Console.WriteLine("==================");
            Console.WriteLine();

            RenderStateMachine(fsm);
        }

        private void RenderStateMachine(FSM fsm)
        {
            // Create a mapping of state identifiers to actual state objects
            var stateMap = fsm.States.ToDictionary(s => s.Identifier, s => s);

            // Find initial state
            var initialState = fsm.States.OfType<InitialState>().FirstOrDefault();
            var finalState = fsm.States.OfType<FinalState>().FirstOrDefault();
            var compoundStates = fsm.States.OfType<CompoundState>().ToList();

            // Render initial state
            if (initialState != null)
            {
                Console.WriteLine($"●─ Initial state ({initialState.Name})");
                Console.WriteLine("   │");

                // Find transition from initial state
                var initialTransition = fsm.Transitions.FirstOrDefault(t => t.Connection.Item1.Identifier == initialState.Identifier);
                if (initialTransition != null)
                {
                    RenderTransitionLabel(initialTransition);
                    Console.WriteLine("   ↓");
                }
                Console.WriteLine();
            }

            // Render compound states and their sub-states
            foreach (var compound in compoundStates)
            {
                RenderCompoundState(compound, fsm, stateMap);
                Console.WriteLine();
            }

            // Render final state
            if (finalState != null)
            {
                Console.WriteLine($"◉─ Final state ({finalState.Name})");
                Console.WriteLine();
            }

            // Render transition summary
            RenderTransitionSummary(fsm, stateMap);
        }

        private void RenderCompoundState(CompoundState compound, FSM fsm, Dictionary<string, IState> stateMap)
        {
            Console.WriteLine($"╔═══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ Compound state: {compound.Name.PadRight(64)} ║");
            Console.WriteLine($"║                                                                                   ║");

            // Render sub-states
            var subStates = compound.SubStates.ToList();
            if (subStates.Any())
            {
                Console.WriteLine($"║   Sub-states:                                                                     ║");
                foreach (var subState in subStates)
                {
                    Console.WriteLine($"║     ○ {subState.Name.PadRight(70)} ║");
                }
                Console.WriteLine($"║                                                                                   ║");

                // Render internal transitions
                var internalTransitions = fsm.Transitions.Where(t =>
                    subStates.Any(s => s.Identifier == t.Connection.Item1.Identifier) &&
                    subStates.Any(s => s.Identifier == t.Connection.Item2.Identifier)).ToList();

                if (internalTransitions.Any())
                {
                    Console.WriteLine($"║   Internal transitions:                                                           ║");
                    foreach (var transition in internalTransitions)
                    {
                        var fromState = stateMap.GetValueOrDefault(transition.Connection.Item1.Identifier);
                        var toState = stateMap.GetValueOrDefault(transition.Connection.Item2.Identifier);
                        var transitionLabel = GetTransitionLabel(transition);

                        if (fromState != null && toState != null)
                        {
                            var line = $"║     {fromState.Name} → {toState.Name} {transitionLabel}";
                            Console.WriteLine(line.PadRight(82) + "║");
                        }
                    }
                    Console.WriteLine($"║                                                                                   ║");
                }
            }

            Console.WriteLine($"╚═══════════════════════════════════════════════════════════════════════════════════╝");

            // Find outgoing transitions from compound state
            var outgoingTransitions = fsm.Transitions.Where(t => t.Connection.Item1.Identifier == compound.Identifier).ToList();
            foreach (var transition in outgoingTransitions)
            {
                Console.WriteLine("   │");
                RenderTransitionLabel(transition);
                Console.WriteLine("   ↓");
            }
        }

        private void RenderTransitionLabel(Transition transition)
        {
            var label = GetTransitionLabel(transition);
            Console.WriteLine($"   │ {label}");
        }

        private string GetTransitionLabel(Transition transition)
        {
            var triggerLabel = !string.IsNullOrEmpty(transition.Trigger) ? transition.Trigger : "";
            var guardLabel = !string.IsNullOrEmpty(transition.Guard) ? $" [{transition.Guard}]" : "";
            return $"{triggerLabel}{guardLabel}";
        }

        private void RenderTransitionSummary(FSM fsm, Dictionary<string, IState> stateMap)
        {
            Console.WriteLine("Transition Summary:");
            Console.WriteLine("-------------------");
            foreach (var transition in fsm.Transitions)
            {
                var fromState = stateMap.GetValueOrDefault(transition.Connection.Item1.Identifier);
                var toState = stateMap.GetValueOrDefault(transition.Connection.Item2.Identifier);
                var label = GetTransitionLabel(transition);

                var fromName = fromState?.Name ?? transition.Connection.Item1.Identifier;
                var toName = toState?.Name ?? transition.Connection.Item2.Identifier;

                Console.WriteLine($"• {fromName} → {toName} {label}");
            }
        }
    }
}