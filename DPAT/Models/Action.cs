namespace DPAT.Models {
    public class Action {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public required ActionType Type { get; set; }
    }
}