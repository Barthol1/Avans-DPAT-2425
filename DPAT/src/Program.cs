// See https://aka.ms/new-console-template for more information
class Program {
    public static void Main(string[] args) {
        Console.WriteLine("Hello, World!");
        var parser = new FSMParser();
        parser.Parse("C:/Users/bart.hol/Documents/Avans-DPAT-2425/lamp.fsm");
    }
}