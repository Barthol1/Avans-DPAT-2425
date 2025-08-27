using DPAT.Domain.Interfaces;

namespace DPAT.Domain
{
    public class Action : IFSMComponent
    {
        public required string Identifier { get; set; }
        public required string Description { get; set; }
        public required ActionType Type { get; set; }

        public void Print()
        {
            Console.WriteLine($"Action: {Identifier} - {Description} - {Type}");
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}