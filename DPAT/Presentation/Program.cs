using DPAT.Application;
using DPAT.Domain;
using DPAT.Infrastructure;

namespace DPAT.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pathArg = args.Length > 0 ? args[0] : Path.Combine(Directory.GetCurrentDirectory(), "../fsm_files/example_lamp.fsm");
            var path = Path.GetFullPath(pathArg);

            var director = new FSMDirector(new FSMBuilder());
            FSM fsm = director.BuildFromFile(path);
            RunValidation(fsm);
            var renderer = new ConsoleRenderer();
            renderer.Render(fsm);
        }

        private static void RunValidation(FSM fsm)
        {
            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());

            validatorService.Validate(fsm);
        }
    }
}