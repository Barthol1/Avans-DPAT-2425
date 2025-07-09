namespace DPAT.Domain.Interfaces
{
    public interface IState : IIdentifier, IDrawable
    {
        public string Name { get; set; }
        public List<IState> Outgoing { get; set; }
        public List<IState> Incoming { get; set; }
    }
}
