using DPAT.Application;
using DPAT.Infrastructure;

namespace DPAT.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new FSMParser();

            var fsm = parser.ParseFile(Path.Combine(AppContext.BaseDirectory, "../../../..", "lamp.fsm")); // temporary path

            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());

            var isValid = validatorService.Validate(fsm);
            Console.WriteLine(isValid);
        }
    }
}