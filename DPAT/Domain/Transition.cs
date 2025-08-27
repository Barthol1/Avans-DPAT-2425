using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Transition : IFSMComponent
    {
        public required string SourceState { get; set; }
        public required string TargetState { get; set; }
        public string? Trigger { get; set; }
        public string? Guard { get; set; }

        public string? EffectActionIdentifier { get; set; }
        public void Print()
        {
            Console.WriteLine($"Transition: {SourceState} -> {TargetState} - {Trigger} - {Guard} - {EffectActionIdentifier}");
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}