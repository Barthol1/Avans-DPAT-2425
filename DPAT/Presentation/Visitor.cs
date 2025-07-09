using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Presentation
{
    public class Visitor : IVisitor
    {
        private readonly Dictionary<string, (int x, int y)> _statePositions = new();
        private readonly List<string> _renderedElements = new();

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
            RenderTransition(transition);
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
            Console.WriteLine($"●─ Initial state ({state.Name})");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderFinalState(FinalState state)
        {
            Console.WriteLine($"◉─ Final state ({state.Name})");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderCompoundState(CompoundState state)
        {
            Console.WriteLine($"╔═══════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ Compound state: {state.Name.PadRight(64)} ║");
            Console.WriteLine($"║                                                                                   ║");
            Console.WriteLine($"║   {RenderSubStates(state)}");
            Console.WriteLine($"║                                                                                   ║");
            Console.WriteLine($"╚═══════════════════════════════════════════════════════════════════════════════════╝");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private string RenderSubStates(CompoundState compound)
        {
            var result = "";
            foreach (var subState in compound.SubStates)
            {
                result += $"○ {subState.Name}".PadRight(40);
            }
            return result.PadRight(75) + "║";
        }

        private void RenderSimpleState(SimpleState state)
        {
            Console.WriteLine($"○─ {state.Name}");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
            // Sub-states are rendered within their parent compound state
        }

        private void RenderGenericState(IState state)
        {
            Console.WriteLine($"○─ {state.Name}");
            _statePositions[state.Identifier] = (0, 0);
            _renderedElements.Add(state.Identifier);
        }

        private void RenderTransition(Transition transition)
        {
            var fromState = transition.Connection.Item1;
            var toState = transition.Connection.Item2;

            var triggerLabel = !string.IsNullOrEmpty(transition.Trigger) ? transition.Trigger : "";
            var guardLabel = !string.IsNullOrEmpty(transition.Guard) ? $" [{transition.Guard}]" : "";

            Console.WriteLine($"   │");
            Console.WriteLine($"   │ {triggerLabel}{guardLabel}");
            Console.WriteLine($"   ↓");
            _renderedElements.Add(transition.Identifier);
        }
    }
}
