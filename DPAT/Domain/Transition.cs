using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Transition
    {
        public required string Id { get; set; }
        public required Tuple<IState, IState> Connection { get; set; }
        public string? Trigger { get; set; }
        public string? Guard { get; set; }
    }
}