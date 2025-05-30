using DPAT.Domain;

namespace DPAT.Infrastructure
{
    // TODO: De parser laten parsen ipv een andere class
    public class FSMParser
    {
        private readonly FSMDirector _director;
        private readonly IFSMBuilder _builder;

        public FSMParser()
        {
            _builder = new FSMBuilder();
            _director = new FSMDirector(_builder);
        }

        public FSM ParseFile(string filePath)
        {
            return _director.ConstructFromFile(filePath);
        }
    }
}