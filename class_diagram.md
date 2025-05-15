```mermaid
classDiagram
    %% --- Core Classes ---
    class Main {
        +main()
    }
    class FiniteStateMachine {
        -List~State~ states
        -List~Transition~ transitions
        -List~Trigger~ registeredTriggers
        -List~Action~ registeredActions
        +addState(state: State) void
        +addTransition(transition: Transition) void
        +addTrigger(trigger: Trigger) void
        +addAction(action: Action) void
        +findStateById(id: String) State
        +findTriggerById(id: String) Trigger
        +findActionById(id: String) Action
        +parseFromFile(filePath: String) FiniteStateMachine
        +getTextualRepresentation() String
        +validateFSM() List~String~
    }

    class State {
        <<Abstract>>
        #identifier: String
        #name: String
        #parentState: CompoundState
        #entryAction: Action
        #doAction: Action
        #exitAction: Action
        #outgoingTransitions: List~Transition~
        #incomingTransitions: List~Transition~
        +getIdentifier() String
        +getName() String
        +getParentState() CompoundState
        +setEntryAction(action: Action) void
        +setDoAction(action: Action) void
        +setExitAction(action: Action) void
        +addOutgoingTransition(t: Transition) void
        +addIncomingTransition(t: Transition) void
        +onEntry()* void
        +onExit()* void
        +doActivity()* void
    }

    class InitialState {
        %% Inherits from State
    }
    class FinalState {
        %% Inherits from State
    }
    class SimpleState {
        %% Inherits from State
    }
    class CompoundState {
        %% Inherits from State
        -List~State~ subStates
        +addSubState(subState: State) void
        +getSubStates() List~State~
    }

    class Transition {
        -identifier: String
        -sourceState: State
        -targetState: State
        -trigger: Trigger  %% Optional
        -guard: GuardCondition  %% Optional
        -effect: Action  %% Optional (Transition Action)
        +getIdentifier() String
        +getSourceState() State
        +getTargetState() State
        +getTrigger() Trigger
        +getGuard() GuardCondition
        +getEffect() Action
        +canFire(eventContext: Object) Boolean
        +executeEffect() void
    }

    class Trigger {
        -identifier: String
        -description: String
        +getIdentifier() String
        +getDescription() String
    }

    class Action {
        -identifier: String
        -description: String
        -type: ActionType
        +getIdentifier() String
        +getDescription() String
        +getType() ActionType
        +execute() void
    }

    class ActionType {
        ENTRY_ACTION
        DO_ACTION
        EXIT_ACTION
        TRANSITION_ACTION
    }

    class GuardCondition {
        -conditionExpression: String %% Stores the guard string from the file
        +getExpression() String
        +evaluate(context: Object) Boolean %% Logic for evaluation
    }

    %% --- Helper/Utility Classes ---
    class FSMParser {
        +parse(filePath: String) FiniteStateMachine
    }

    class AbstractView {
        <<Interface>>
        +display(fsm: FiniteStateMachine) void
    }
    class ConsoleView {
        %% Implements AbstractView
        +display(fsm: FiniteStateMachine) void
    }
    
    class IValidator {
        <<Interface>>
        +validate(fsm: FiniteStateMachine) List~String~ %% Returns list of error descriptions
    }
    class NonDeterministicValidator {
        %% Implements IValidator
        +validate(fsm: FiniteStateMachine) List~String~
    }
    class InitialFinalStateValidator {
        %% Implements IValidator
        +validate(fsm: FiniteStateMachine) List~String~
    }
    class StructureValidator { 
        %% Implements IValidator
        %% For TransitionToCompound or UnreachableState
        +validate(fsm: FiniteStateMachine) List~String~
    }

    %% --- Relationships ---
    Main --> FSMParser: creates
    FiniteStateMachine "1" *--> "0..*" State : holds
    FiniteStateMachine "1" *--> "0..*" Transition : contains
    FiniteStateMachine "1" o--> "0..*" Trigger : registers
    FiniteStateMachine "1" o--> "0..*" Action : registers

    State <|-- InitialState : inherits from
    State <|-- FinalState : inherits from
    State <|-- SimpleState : inherits from
    State <|-- CompoundState : inherits from

    CompoundState "1" *--> "0..*" State : contains
    State "1" --> "0..1" CompoundState : has parent

    Transition "1" --> "1" State : has source
    Transition "1" --> "1" State : has target
    Transition "1" --> "0..1" Trigger : has optional trigger
    Transition "1" --> "0..1" GuardCondition : has optional guard
    Transition "1" --> "0..1" Action : has optional effect

    State "1" --> "0..1" Action : has entry action
    State "1" --> "0..1" Action : has do action
    State "1" --> "0..1" Action : has exit action
    
    Action "1" --> "1" ActionType : has type

    FSMParser ..> FiniteStateMachine : creates
    
    AbstractView <|.. ConsoleView : implements
    ConsoleView ..> FiniteStateMachine : displays

    IValidator <|.. NonDeterministicValidator : implements
    IValidator <|.. InitialFinalStateValidator : implements
    IValidator <|.. StructureValidator : implements
    NonDeterministicValidator ..> FiniteStateMachine : validates
    InitialFinalStateValidator ..> FiniteStateMachine : validates
    StructureValidator ..> FiniteStateMachine : validates

