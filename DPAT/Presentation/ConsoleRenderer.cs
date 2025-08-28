using DPAT.Domain;
using DPAT.Domain.Interfaces;

namespace DPAT.Presentation
{
    public class ConsoleRenderer : IRenderer
    {
        public void Render(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}