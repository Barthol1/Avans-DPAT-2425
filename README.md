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
	class IStateFactory {
		+Create(String type, String identifier, String name) IState
	}
	class IFSMValidator {
		+Validate(FSM fsm) void
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
		+String Trigger
		+String Guard
		+String EffectActionIdentifier
		+String Identifier
		+Accept(IVisitor visitor) void
	}
	class InitialState {
		+String Name
		+String Identifier
		+Accept(IVisitor visitor) void
	}
	class SimpleState {
		+String Name
		+String Identifier
		+Accept(IVisitor visitor) void
	}
	class CompoundState {
		+String Name
		+String Identifier
		+List~IState~ SubStates
		+Accept(IVisitor visitor) void
	}
	class FinalState {
		+String Name
		+String Identifier
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
		+Transition GetTransition(String line, IEnumerable~IState~ states)
		+Action GetAction(String line)
		+Trigger GetTrigger(String line)
	}
	class ValidatorService {
		-List~IFSMValidator~ _validators
		+AddValidator(IFSMValidator validator) void
		+Validate(FSM fsm) void
	}
	class DeterministicValidator {
		+Validate(FSM fsm) void
	}
	class FinalStateOutgoingValidator {
		+Validate(FSM fsm) void
	}
	class InitialIngoingValidator {
		+Validate(FSM fsm) void
	}
	class TransitionTargetValidator {
		+Validate(FSM fsm) void
	}
	class UnreachableStateValidator {
		+Validate(FSM fsm) void
	}
	class ConsoleRenderer {
		+Render(FSM fsm) void
	}
	class SimpleStateFactory {
		+Create(String type, String identifier, String name) IState
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
	<<Interface>> IStateFactory
	<<Interface>> IFSMValidator
	<<Interface>> IRenderer
	<<Enumeration>> ActionType

	IIdentifier <|-- IState
	IIdentifier <|-- Action
	IIdentifier <|-- Trigger
	IIdentifier <|-- Transition

	IDrawable <|-- IState
	IDrawable <|-- Action
	IDrawable <|-- Trigger
	IDrawable <|-- Transition

	FSM o-- "many" IState
	FSM o-- "many" Action
	FSM o-- "many" Trigger
	FSM o-- "many" Transition

	IState <|-- InitialState
	IState <|-- SimpleState
	IState <|-- CompoundState
	IState <|-- FinalState
	CompoundState o-- "many" IState

	IFSMBuilder <|-- FSMBuilder
	IStateFactory <|-- SimpleStateFactory

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
	IFSMValidator <|-- UnreachableStateValidator

	IRenderer <|-- ConsoleRenderer
	IVisitor <|-- Visitor

	Program ..> FSMDirector
	Program ..> FSMBuilder
	Program ..> ValidatorService
	Program ..> ConsoleRenderer
	Program ..> IFSMValidator
```
