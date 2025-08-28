namespace DPAT.Domain.Interfaces
{
    public interface IFSMComponent
    {
        void Print(IVisitor visitor);
    }
}