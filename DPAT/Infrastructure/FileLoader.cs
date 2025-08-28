using DPAT.Domain.Interfaces;

namespace DPAT.Infrastructure
{
    public class FileLoader : ILoader
    {
        public List<string> Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }

            return File.ReadLines(path).ToList();
        }
    }
}