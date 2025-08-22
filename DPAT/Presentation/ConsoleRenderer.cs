using DPAT.Domain;

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
            Console.WriteLine("####################################################################");
            Console.WriteLine("# Diagram: FSM");
            Console.WriteLine("####################################################################");
            Console.WriteLine();

            var visitor = new Visitor(fsm);

            var initial = fsm.States.OfType<InitialState>().FirstOrDefault();
            if (initial != null)
            {
                initial.Accept(visitor);
            }

            var compoundStates = fsm.States.OfType<CompoundState>().ToList();
            var childIds = new HashSet<string>(compoundStates.SelectMany(c => c.SubStates.Select(s => s.Identifier)));

            foreach (var compound in compoundStates)
            {
                if (childIds.Contains(compound.Identifier)) continue;
                compound.Accept(visitor);
            }

            var final = fsm.States.OfType<FinalState>().FirstOrDefault();
            if (final != null)
            {
                Console.WriteLine();
                final.Accept(visitor);
            }

            // Print all the collected output lines
            foreach (var line in visitor.OutputLines)
            {
                Console.WriteLine(line);
            }
        }
    }
}