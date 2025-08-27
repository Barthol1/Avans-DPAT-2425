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
        public void Visit(State state)
        {
            throw new NotImplementedException();
        }

        public void Visit(Transition transition)
        {
            throw new NotImplementedException();
        }

        public void Visit(Trigger trigger)
        {
            throw new NotImplementedException();
        }

        public void Visit(Action action)
        {
            throw new NotImplementedException();
        }
    }
}

