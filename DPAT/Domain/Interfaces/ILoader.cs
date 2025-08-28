namespace DPAT.Domain.Interfaces
{
    public interface ILoader
    {
        List<string> Load(string path);
    }
}