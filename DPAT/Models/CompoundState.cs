namespace DPAT.Models {
    class CompoundState: IState {
    public string Name { get; set; }
    public IState? Parent { get; set; }
    public string Identifier { get; set; }
    public List<IState> SubStates { get; set; }
    
    public CompoundState(string identifier, string name) {
        Identifier = identifier;
        Name = name;
        SubStates = new List<IState>();
    }
}
}