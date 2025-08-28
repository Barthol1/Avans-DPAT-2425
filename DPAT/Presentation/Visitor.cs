using System;
using System.Collections.Generic;
using System.Linq;
using DPAT.Domain;
using DPAT.Domain.Interfaces;
using Action = DPAT.Domain.Action;

namespace DPAT.Presentation
{
    public class DrawConsoleVisitor : IVisitor
    {
        public void Print(State state)
        {
            Console.WriteLine($"State: - {state.Name} - {state.Type}");
        }

        public void Print(Transition transition)
        {
            Console.WriteLine($"Transition: {transition.SourceState} -> {transition.TargetState} - {transition.Trigger} - {transition.Guard}");
        }

        public void Print(Trigger trigger)
        {
            Console.WriteLine($"Trigger: {trigger.Description}");
        }

        public void Print(Action action)
        {
            Console.WriteLine($"Action: - {action.Description} - {action.Type}");
        }
    }
}

