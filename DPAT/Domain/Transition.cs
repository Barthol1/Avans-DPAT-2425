using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Transition : IIdentifier, IDrawable
    {
        public required Tuple<IState, IState> Connection { get; set; }
        public string? Trigger { get; set; }
        public string? Guard { get; set; }

        public string? EffectActionIdentifier { get; set; }
        public required string Identifier { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}