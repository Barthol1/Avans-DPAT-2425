using DPAT.Application;
using DPAT.Infrastructure;

namespace DPAT.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = args.Length > 0 ? Path.GetFullPath(args[0]) : Path.Combine(Directory.GetCurrentDirectory(), "../fsm_files/example_lamp.fsm");

            var parser = new FSMParser();

            var fsm = parser.ParseFile(path);

            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());

            var isValid = validatorService.Validate(fsm);
            Console.WriteLine(isValid);

            IRenderer renderer = new ConsoleRenderer();
            renderer.Render(fsm);
        }
    }
}