using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Presentation
{
    public class ConsoleRenderer : IRenderer
    {
        List<IDrawable> drawables = new List<IDrawable>();

        public ConsoleRenderer()
        {
            
        }

        public void Render(FSM fsm)
        {
            var visitor = new Visitor();
            drawables.AddRange(fsm.States);
            drawables.AddRange(fsm.Actions);
            drawables.AddRange(fsm.Triggers);
            drawables.AddRange(fsm.Transitions);

            foreach (var drawable in drawables)
            {
                drawable.Accept(visitor);
            }
        }
    }
}