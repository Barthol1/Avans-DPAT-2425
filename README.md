# Avans-DPAT-2425

| Naam         | Studentennummer |
| ------------ | --------------- |
| Bart Hol     | 2171763         |
| Roel Leijser | 2168562         |

## Rubric

| Criteria        | Score     |
| --------------- | --------- |
| Creatiepatronen | Goed      |
| Structuurpatronen | Goed |
| Gedragspatronen | Goed |
| Modulaire begrijpelijkheid Goede/consistente naamgevingen, code blocks doen slechts één ding. | Ruim voldoende |
| Modulaire compositie/decompositie Code blocks zijn onafhankelijk en herbruikbaar. | Goed |
| Kwaliteit - Code smells | Goed |
| Kwaliteit - Testing | Goed |
| Nice-to-have | NVT |


## Main Class Diagram

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

## Software Design Patterns

### 1. Builder Pattern

```mermaid
classDiagram
    class IFSMBuilder {
        <<interface>>
        +AddState(IState state) void
        +AddAction(Action action) void
        +AddTrigger(Trigger trigger) void
        +AddTransition(Transition transition) void
        +FSM Build() FSM
        +Reset() void
    }
    
    class FSMBuilder {
        -FSM _fsm
        +AddState(IState state) void
        +AddAction(Action action) void
        +AddTrigger(Trigger trigger) void
        +AddTransition(Transition transition) void
        +FSM Build() FSM
        +Reset() void
    }
    
    class FSMDirector {
        -IFSMBuilder _builder
        -FSMParser _parser
        +ChangeBuilder(IFSMBuilder builder) void
        +FSM Build() FSM
        +FSM BuildFromFile(String filePath) FSM
    }
    
    class FSM {
        +List~IState~ States
        +List~Action~ Actions
        +List~Trigger~ Triggers
        +List~Transition~ Transitions
    }
    
    IFSMBuilder <|.. FSMBuilder
    FSMDirector o-- IFSMBuilder
    FSMDirector ..> FSM
    FSMBuilder ..> FSM
```

### 2. Factory Method Pattern

```mermaid
classDiagram
    class IStateFactory {
        <<interface>>
        +Create(String type, String identifier, String name) IState
    }
    
    class SimpleStateFactory {
        +Create(String type, String identifier, String name) IState
    }
    
    class IState {
        <<interface>>
        +String Name
    }
    
    class InitialState {
        +String Name
        +String Identifier
    }
    
    class SimpleState {
        +String Name
        +String Identifier
    }
    
    class CompoundState {
        +String Name
        +String Identifier
        +List~IState~ SubStates
    }
    
    class FinalState {
        +String Name
        +String Identifier
    }
    
    IStateFactory <|.. SimpleStateFactory
    SimpleStateFactory ..> IState
    IState <|-- InitialState
    IState <|-- SimpleState
    IState <|-- CompoundState
    IState <|-- FinalState
```

### 3. Composite Pattern

```mermaid
classDiagram
    class IState {
        <<interface>>
        +String Name
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
        +AddSubState(IState subState) void
    }
    
    class InitialState {
        +String Name
        +String Identifier
        +Accept(IVisitor visitor) void
    }
    
    class FinalState {
        +String Name
        +String Identifier
        +Accept(IVisitor visitor) void
    }
    
    IState <|-- SimpleState
    IState <|-- CompoundState
    IState <|-- InitialState
    IState <|-- FinalState
    CompoundState o-- "many" IState
```

### 4. Visitor Pattern

```mermaid
classDiagram
    class IVisitor {
        <<interface>>
        +Visit(IState state) void
        +Visit(Transition transition) void
        +Visit(Trigger trigger) void
        +Visit(Action action) void
    }
    
    class Visitor {
        +Visit(IState state) void
        +Visit(Transition transition) void
        +Visit(Trigger trigger) void
        +Visit(Action action) void
    }
    
    class IDrawable {
        <<interface>>
        +Accept(IVisitor visitor) void
    }
    
    class IState {
        <<interface>>
        +String Name
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
    
    IVisitor <|.. Visitor
    IDrawable <|-- IState
    IDrawable <|-- Transition
    IDrawable <|-- Action
    IDrawable <|-- Trigger
    Visitor ..> IState
    Visitor ..> Transition
    Visitor ..> Action
    Visitor ..> Trigger
```

### 5. Strategy Pattern

```mermaid
classDiagram
    class IFSMValidator {
        <<interface>>
        +Validate(FSM fsm) void
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
    
    class FSM {
        +List~IState~ States
        +List~Action~ Actions
        +List~Trigger~ Triggers
        +List~Transition~ Transitions
    }
    
    IFSMValidator <|.. DeterministicValidator
    IFSMValidator <|.. FinalStateOutgoingValidator
    IFSMValidator <|.. InitialIngoingValidator
    IFSMValidator <|.. TransitionTargetValidator
    IFSMValidator <|.. UnreachableStateValidator
    ValidatorService o-- "many" IFSMValidator
    ValidatorService ..> FSM
    DeterministicValidator ..> FSM
    FinalStateOutgoingValidator ..> FSM
    InitialIngoingValidator ..> FSM
    TransitionTargetValidator ..> FSM
    UnreachableStateValidator ..> FSM
```
