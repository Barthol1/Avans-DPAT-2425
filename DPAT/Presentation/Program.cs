using DPAT.Application;
using DPAT.Domain;
using DPAT.Infrastructure;

namespace DPAT.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = args.Length > 0 ? Path.GetFullPath(args[0]) : Path.Combine(Directory.GetCurrentDirectory(), "../fsm_files/invalid_compound.fsm");

            var FSMDirector = new FSMDirector(new FSMBuilder());
            FSM fsm = FSMDirector.BuildFromFile(path);

            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());

            var isValid = validatorService.Validate(fsm);
            Console.WriteLine(isValid);

            var renderer = new ConsoleRenderer();
            renderer.Render(fsm);
        }
    }
}