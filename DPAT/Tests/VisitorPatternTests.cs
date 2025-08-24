using DPAT.Domain;
using DPAT.Domain.Interfaces;
using DPAT.Presentation;
using Xunit;
using Action = DPAT.Domain.Action;

namespace DPAT.Tests
{
    public class VisitorPatternTests
    {
        [Fact]
        public void Visitor_Should_Implement_Double_Dispatch_Correctly()
        {
            var fsm = new FSM
            {
                States = new List<IState>
                {
                    new InitialState("init", "Start"),
                    new SimpleState("simple", "Simple"),
                    new FinalState("final", "End")
                },
                Transitions = new List<Transition>(),
                Triggers = new List<Trigger>(),
                Actions = new List<Action>()
            };

            var visitor = new Visitor(fsm);
            var initialState = fsm.States.OfType<InitialState>().First();
            var simpleState = fsm.States.OfType<SimpleState>().First();
            var finalState = fsm.States.OfType<FinalState>().First();


            Assert.Null(Record.Exception(() => initialState.Accept(visitor)));
            Assert.Null(Record.Exception(() => simpleState.Accept(visitor)));
            Assert.Null(Record.Exception(() => finalState.Accept(visitor)));
        }

        [Fact]
        public void Visitor_Should_Throw_When_Calling_Visit_IState_Directly()
        {
            var fsm = new FSM
            {
                States = new List<IState>(),
                Transitions = new List<Transition>(),
                Triggers = new List<Trigger>(),
                Actions = new List<Action>()
            };

            var visitor = new Visitor(fsm);
            var initialState = new InitialState("test", "Test");

            Assert.Throws<InvalidOperationException>(() => visitor.Visit((IState)initialState));
        }
    }
}
