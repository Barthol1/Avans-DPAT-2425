class FSMParser {
    public void Parse(string filePath) {
        var lines = File.ReadAllLines(filePath);
        var states = new List<IState>();

        foreach (var line in lines) {
            System.Console.WriteLine(line);
        }
    }
}


