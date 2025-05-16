namespace DPAT.Models {
    public class FSM {
        public required List<IState> States { get; set; }
        public required List<Transition> Transitions { get; set; }
        public required List<Action> Actions { get; set; }
        public required List<Trigger> Triggers { get; set; }
    }
}