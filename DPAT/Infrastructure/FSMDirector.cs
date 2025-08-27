using DPAT.Domain.Interfaces;

namespace DPAT.Infrastructure
{
    class FSMDirector
    {
        private IFSMBuilder _builder;

        public FSMDirector(IFSMBuilder builder)
        {
            _builder = builder;
        }

        public void ChangeBuilder(IFSMBuilder builder)
        {
            _builder = builder;
        }

        public IFSMComponent BuildFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            var parser = new FSMParser();
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                    continue;

                if (line.StartsWith("STATE"))
                    _builder.AddState(parser.GetState(line));
                else if (line.StartsWith("TRANSITION"))
                    _builder.AddTransition(parser.GetTransition(line));
                else if (line.StartsWith("ACTION"))
                    _builder.AddAction(parser.GetAction(line));
                else if (line.StartsWith("TRIGGER"))
                    _builder.AddTrigger(parser.GetTrigger(line));
                else
                    throw new FormatException($"Invalid line: {line}");
            }
            return _builder.Build();
        }
    }
}