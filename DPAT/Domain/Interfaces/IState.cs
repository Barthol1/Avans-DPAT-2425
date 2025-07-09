namespace DPAT.Domain.Interfaces
{
    public interface IState : IIdentifier, IDrawable
    {
        public string Name { get; set; }
    }
}
