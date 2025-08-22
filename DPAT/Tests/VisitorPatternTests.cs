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
            // Arrange
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

            // Act & Assert - These should not throw exceptions and should call the correct Visit method
            Assert.Null(Record.Exception(() => initialState.Accept(visitor)));
            Assert.Null(Record.Exception(() => simpleState.Accept(visitor)));
            Assert.Null(Record.Exception(() => finalState.Accept(visitor)));
        }

        [Fact]
        public void Visitor_Should_Throw_When_Calling_Visit_IState_Directly()
        {
            // Arrange
            var fsm = new FSM
            {
                States = new List<IState>(),
                Transitions = new List<Transition>(),
                Triggers = new List<Trigger>(),
                Actions = new List<Action>()
            };

            var visitor = new Visitor(fsm);
            var initialState = new InitialState("test", "Test");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => visitor.Visit((IState)initialState));
        }

        [Fact]
        public void State_Classes_Should_Have_Accept_Method()
        {
            // Arrange
            var initialState = new InitialState("init", "Start");
            var simpleState = new SimpleState("simple", "Simple");
            var finalState = new FinalState("final", "End");
            var compoundState = new CompoundState("compound", "Compound");

            // Act & Assert
            Assert.NotNull(initialState.GetType().GetMethod("Accept"));
            Assert.NotNull(simpleState.GetType().GetMethod("Accept"));
            Assert.NotNull(finalState.GetType().GetMethod("Accept"));
            Assert.NotNull(compoundState.GetType().GetMethod("Accept"));
        }

        [Fact]
        public void IDrawable_Interface_Should_Declare_Accept_Method()
        {
            // Arrange & Act
            var acceptMethod = typeof(IDrawable).GetMethod("Accept");

            // Assert
            Assert.NotNull(acceptMethod);
            Assert.Equal(typeof(IVisitor), acceptMethod.GetParameters()[0].ParameterType);
        }
    }
}
