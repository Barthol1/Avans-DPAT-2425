// See https://aka.ms/new-console-template for more information
class Program {
    public static void Main(string[] args) {
        var parser = new FSMParser();
        var fsm = parser.ParseFile("C:/Users/bart.hol/Documents/Avans-DPAT-2425/lamp.fsm");
    }
}