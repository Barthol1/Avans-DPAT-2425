namespace DPAT.Domain.Interfaces
{
    public interface IFSMComponentFactory
    {
        IFSMComponent Create(string type, string identifier, string name);
    }
}



