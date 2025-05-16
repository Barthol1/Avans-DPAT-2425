namespace DPAT.Models {
    public interface IState: IIdentifier {
        public string Name { get; set; }
        public IState? Parent { get; set; }
    }
}
