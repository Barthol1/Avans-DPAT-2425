using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Presentation
{
    public class Visitor : IVisitor
    {
        public void Visit(IState state)
        {
            Console.WriteLine(state.Identifier);
        }

        public void Visit(Transition transition)
        {
            Console.WriteLine(transition.Identifier);
        }

        public void Visit(Trigger trigger)
        {
            Console.WriteLine(trigger.Identifier);
        }

        public void Visit(Action action)
        {
            Console.WriteLine(action.Identifier);
        }
    }
}
