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
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Usage: DPAT <path-to-fsm-file>");
                Environment.Exit(1);
                return;
            }
            var path = Path.GetFullPath(args[0]);
            var director = new FSMDirector(new FSMBuilder());

            IFSMComponent fsm = director.BuildFromFile(path);

            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());
            validatorService.Validate(fsm);

            fsm.Print(new DrawConsoleVisitor());
        }




    }
}