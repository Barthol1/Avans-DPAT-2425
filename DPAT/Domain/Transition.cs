using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Transition
    {
        public required string Id { get; set; }
        public required IState Source { get; set; }
        public required IState Destination { get; set; }
        public string? Trigger { get; set; }
        public string? Guard { get; set; }
    }
}