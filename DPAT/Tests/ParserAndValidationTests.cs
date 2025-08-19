using System;
using System.IO;
using DPAT.Application;
using DPAT.Domain;
using DPAT.Infrastructure;
using Xunit;

namespace DPAT.Tests
{
    public class ParserAndValidationTests
    {
        private static string ResolvePath(string relative)
        {
            var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            return Path.Combine(root, "fsm_files", relative);
        }

        private static FSM Parse(string fileName)
        {
            var director = new FSMDirector(new FSMBuilder());
            return director.BuildFromFile(ResolvePath(fileName));
        }

        private static void ValidateAll(FSM fsm)
        {
            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());
            validatorService.Validate(fsm);
        }

        [Fact]
        public void ParsesAndBuildsHierarchy_UserAccount()
        {
            var fsm = Parse("example_user_account.fsm");
            var created = fsm.States.Find(s => s.Identifier == "created") as CompoundState;
            Assert.NotNull(created);
            Assert.Contains(created.SubStates, s => s.Identifier == "inactive");
            var inactive = fsm.States.Find(s => s.Identifier == "inactive") as CompoundState;
            Assert.NotNull(inactive);
            Assert.Contains(inactive.SubStates, s => s.Identifier == "unverified");
        }

        [Fact]
        public void Invalid_TargetsCompound_Throws()
        {
            var fsm = Parse("invalid_compound.fsm");
            var service = new ValidatorService();
            service.AddValidator(new TransitionTargetValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }

        [Fact]
        public void Invalid_InitialIncoming_Throws()
        {
            var fsm = Parse("invalid_initial.fsm");
            var service = new ValidatorService();
            service.AddValidator(new InitialIngoingValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }

        [Fact]
        public void Invalid_FinalOutgoing_Throws()
        {
            var fsm = Parse("invalid_final.fsm");
            var service = new ValidatorService();
            service.AddValidator(new FinalStateOutgoingValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }

        [Fact]
        public void Invalid_NonDeterministic_SameTrigger_Throws()
        {
            var fsm = Parse("invalid_deterministic1.fsm");
            var service = new ValidatorService();
            service.AddValidator(new DeterministicValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }

        [Fact]
        public void Invalid_NonDeterministic_SameGuard_Throws()
        {
            var fsm = Parse("invalid_deterministic2.fsm");
            var service = new ValidatorService();
            service.AddValidator(new DeterministicValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }

        [Fact]
        public void Valid_Deterministic_Passes()
        {
            var fsm = Parse("valid_deterministic.fsm");
            var service = new ValidatorService();
            service.AddValidator(new DeterministicValidator());
            service.Validate(fsm);
        }

        [Fact]
        public void Invalid_Unreachable_Throws()
        {
            var fsm = Parse("invalid_unreachable.fsm");
            var service = new ValidatorService();
            service.AddValidator(new UnreachableStateValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }
    }
}



