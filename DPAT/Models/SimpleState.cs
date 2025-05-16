namespace DPAT.Models
{
    class SimpleState : IState
    {
        public string Name { get; set; }
        public IState? Parent { get; set; }
        public string Identifier { get; set; }

        public SimpleState(string identifier, string name)
        {
            Identifier = identifier;
            Name = name;
        }
    }
}
