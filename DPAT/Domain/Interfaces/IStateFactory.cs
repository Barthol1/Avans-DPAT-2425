namespace DPAT.Domain.Interfaces
{
    public interface IStateFactory
    {
        IState Create(string type, string identifier, string name);
    }
}



