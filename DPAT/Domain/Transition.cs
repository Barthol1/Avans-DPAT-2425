using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Transition : IFSMComponent
    {
        public required Tuple<IState, IState> Connection { get; set; }
        public string? Trigger { get; set; }
        public string? Guard { get; set; }

        public string? EffectActionIdentifier { get; set; }
        public void Print()
        {
            throw new NotImplementedException();
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}