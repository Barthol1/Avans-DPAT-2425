using DPAT.Application;
using DPAT.Domain;
using DPAT.Domain.Interfaces;
using DPAT.Infrastructure;

namespace DPAT.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILoader loader = new FileLoader();
            var filePath = args[0];
            var lines = loader.Load(filePath);
            var director = new FSMDirector(new FSMBuilder());

            IFSMComponent fsm = director.Make(lines);

            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            // validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            // validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());
            validatorService.Validate((FSM)fsm);

            fsm.Print(new DrawConsoleVisitor());
        }




    }
}