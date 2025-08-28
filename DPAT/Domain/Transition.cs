using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Transition : IFSMComponent
    {
        public required State SourceState { get; set; }
        public required State TargetState { get; set; }
        public string? Trigger { get; set; }
        public Action? Action { get; set; }
        public string? Guard { get; set; }

        public void Print(IVisitor visitor)
        {
            visitor.Print(this);
        }
    }
}