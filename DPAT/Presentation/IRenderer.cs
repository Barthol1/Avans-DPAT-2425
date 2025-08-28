using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Presentation
{
    public interface IRenderer
    {
        void Render(IFSMComponent fsm);
    }
}