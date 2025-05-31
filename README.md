# Avans-DPAT-2425

| Naam         | Studentennummer |
| ------------ | --------------- |
| Bart Hol     | 2171763         |
| Roel Leijser | 2168562         |

```mermaid
classDiagram
direction LR
    class IIdentifier {
	    +String Identifier
    }
    class IDrawable {
	    +Accept(IVisitor visitor) void
    }
    class IState {
	    +String Name
	    +IState Parent
	    +List~IState~ Outgoing
	    +List~IState~ Incoming
	    +Execute() void
    }
    class IVisitor {
	    +Visit(IState state) void
	    +Visit(Transition transition) void
	    +Visit(Trigger trigger) void
	    +Visit(Action action) void
    }
    class IFSMBuilder {
	    +AddState(IState state) void
	    +AddAction(Action action) void
	    +AddTrigger(Trigger trigger) void
	    +AddTransition(Transition transition) void
	    +FSM Build()
	    +Reset() void
    }
    class IFSMValidator {
	    +Validate(FSM fsm) bool
    }
    class IRenderer {
	    +Render(FSM fsm) void
    }
    class ActionType {
	    ENTRY_ACTION
	    DO_ACTION
	    EXIT_ACTION
	    TRANSITION_ACTION
    }
    class FSM {
	    +List~IState~ States
	    +List~Action~ Actions
	    +List~Trigger~ Triggers
	    +List~Transition~ Transitions
    }
    class Action {
	    +String Identifier
	    +String Description
	    +ActionType Type
	    +Accept(IVisitor visitor) void
    }
    class Trigger {
	    +String Identifier
	    +String Description
	    +Accept(IVisitor visitor) void
    }
    class Transition {
	    +Tuple~IState,IState~ Connection
	    +String TriggerIdentifier
	    +String Guard
	    +String Identifier
	    +Accept(IVisitor visitor) void
    }
    class InitialState {
	    +String Name
	    +IState Parent
	    +String Identifier
	    +List~IState~ Outgoing
	    +List~IState~ Incoming
	    +Accept(IVisitor visitor) void
    }
    class SimpleState {
	    +String Name
	    +IState Parent
	    +String Identifier
	    +List~IState~ Outgoing
	    +List~IState~ Incoming
	    +Accept(IVisitor visitor) void
    }
    class CompoundState {
	    +String Name
	    +IState Parent
	    +String Identifier
	    +List~IState~ SubStates
	    +List~IState~ Outgoing
	    +List~IState~ Incoming
	    +Accept(IVisitor visitor) void
    }
    class FinalState {
	    +String Name
	    +IState Parent
	    +String Identifier
	    +List~IState~ Outgoing
	    +List~IState~ Incoming
	    +Accept(IVisitor visitor) void
    }
    class FSMBuilder {
	    -FSM _fsm
	    +AddState(IState state) void
	    +AddAction(Action action) void
	    +AddTrigger(Trigger trigger) void
	    +AddTransition(Transition transition) void
	    +FSM Build()
	    +Reset() void
    }
    class FSMDirector {
	    -IFSMBuilder _builder
	    -FSMParser _parser
	    +ChangeBuilder(IFSMBuilder builder) void
	    +FSM Build()
	    +FSM BuildFromFile(String filePath)
    }
    class FSMParser {
	    +IState GetState(String line)
	    +Transition GetTransition(String line)
	    +Action GetAction(String line)
	    +Trigger GetTrigger(String line)
    }
    class ValidatorService {
	    -List~IFSMValidator~ _validators
	    +AddValidator(IFSMValidator validator) void
	    +Validate(FSM fsm) bool
    }
    class DeterministicValidator {
	    +Validate(FSM fsm) bool
    }
    class FinalStateOutgoingValidator {
	    +Validate(FSM fsm) bool
    }
    class InitialIngoingValidator {
	    +Validate(FSM fsm) bool
    }
    class TransitionTargetValidator {
	    +Validate(FSM fsm) bool
    }
    class UnreachableStateValidator {
	    +Validate(FSM fsm) bool
    }
    class ConsoleRenderer {
	    -List~IDrawable~ drawables
	    +Render(FSM fsm) void
    }
    class Visitor {
	    +Visit(IState state) void
	    +Visit(Transition transition) void
	    +Visit(Trigger trigger) void
	    +Visit(Action action) void
    }
    class Program {
	    +Main(String[] args) void
    }

	<<Interface>> IIdentifier
	<<Interface>> IDrawable
	<<Interface>> IState
	<<Interface>> IVisitor
	<<Interface>> IFSMBuilder
	<<Interface>> IFSMValidator
	<<Interface>> IRenderer
	<<Enumeration>> ActionType

    IIdentifier <|-- IState
    IDrawable <|-- IState
    FSM o-- "many" IState
    FSM o-- "many" Action
    FSM o-- "many" Trigger
    FSM o-- "many" Transition
    IIdentifier <|-- Action
    IDrawable <|-- Action
    Action o-- ActionType
    IIdentifier <|-- Trigger
    IDrawable <|-- Trigger
    IIdentifier <|-- Transition
    IDrawable <|-- Transition
    Transition --> "2" IState
    IState <|-- InitialState
    IState <|-- SimpleState
    IState <|-- CompoundState
    CompoundState o-- "many" IState
    IState <|-- FinalState
    IFSMBuilder <|-- FSMBuilder
    FSMBuilder ..> FSM
    FSMDirector o-- IFSMBuilder
    FSMDirector o-- FSMParser
    FSMDirector ..> FSM
    FSMParser ..> IState
    FSMParser ..> Transition
    FSMParser ..> Action
    FSMParser ..> Trigger
    ValidatorService o-- "many" IFSMValidator
    ValidatorService ..> FSM
    IFSMValidator <|-- DeterministicValidator
    IFSMValidator <|-- FinalStateOutgoingValidator
    IFSMValidator <|-- InitialIngoingValidator
    IFSMValidator <|-- TransitionTargetValidator
    TransitionTargetValidator ..> CompoundState
    IFSMValidator <|-- UnreachableStateValidator
    IRenderer <|-- ConsoleRenderer
    ConsoleRenderer ..> FSM
    ConsoleRenderer ..> Visitor
    ConsoleRenderer ..> IDrawable
    IVisitor <|-- Visitor
    Program ..> FSMDirector
    Program ..> FSMBuilder
    Program ..> ValidatorService
    Program ..> ConsoleRenderer
    Program ..> IFSMValidator
    IDrawable "1" ..> "1" IVisitor

```
