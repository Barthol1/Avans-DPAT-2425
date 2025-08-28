using System;
using System.IO;
using DPAT.Application;
using DPAT.Domain;
using DPAT.Infrastructure;
using DPAT.Domain.Interfaces;
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
            var loader = new FileLoader();
            var lines = loader.Load(ResolvePath(fileName));
            return (FSM)director.Make(lines);
        }

        private static void ValidateAll(FSM fsm)
        {
            var validatorService = new ValidatorService();
            validatorService.AddValidator(new DeterministicValidator());
            // validatorService.AddValidator(new UnreachableStateValidator());
            validatorService.AddValidator(new TransitionTargetValidator());
            // validatorService.AddValidator(new InitialIngoingValidator());
            validatorService.AddValidator(new FinalStateOutgoingValidator());
            validatorService.Validate(fsm);
        }

        [Fact]
        public void Invalid_TargetsCompound_Throws()
        {
            var fsm = Parse("invalid_compound.fsm");
            var service = new ValidatorService();
            service.AddValidator(new TransitionTargetValidator());
            Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        }

        // [Fact]
        // public void Invalid_InitialIncoming_Throws()
        // {
        //     var fsm = Parse("invalid_initial.fsm");
        //     var service = new ValidatorService();
        //     service.AddValidator(new InitialIngoingValidator());
        //     Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        // }

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
        public void Invalid_NonDeterministic_AutomaticTransition_Throws()
        {
            var fsm = Parse("invalid_deterministic3.fsm");
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

        // [Fact]
        // public void Invalid_Unreachable_Throws()
        // {
        //     var fsm = Parse("invalid_unreachable.fsm");
        //     var service = new ValidatorService();
        //     service.AddValidator(new UnreachableStateValidator());
        //     Assert.Throws<InvalidOperationException>(() => service.Validate(fsm));
        // }

        [Fact]
        public void ParsesAndValidates_LampExample_Passes()
        {
            var fsm = Parse("example_lamp.fsm");
            ValidateAll(fsm);
        }

        [Fact]
        public void ParsesStateAndReturnsTuple()
        {
            var parser = new FSMParser();
            var result = parser.ParseState("""STATE initial _ "powered off" : INITIAL;""");
            Assert.Equal(typeof(ParsedState), result.GetType());
            Assert.Equal("initial", result.Identifier);
            Assert.Equal("powered off", result.Name);
            Assert.Equal("INITIAL", result.Type.ToString());
        }

        [Fact]
        public void ParsesTransitionAndReturnsTuple()
        {
            var parser = new FSMParser();
            var result = parser.ParseTransition("""TRANSITION t1 initial -> off power_on "";""");
            Assert.Equal(typeof(ParsedTransition), result.GetType());
            Assert.Equal("t1", result.Identifier);
            Assert.Equal("initial", result.SourceId);
            Assert.Equal("off", result.TargetId);
            Assert.Equal("power_on", result.TriggerName);
            Assert.Equal("", result.GuardCondition);
        }

        [Fact]
        public void ParsesTriggerAndReturnsTuple()
        {
            var parser = new FSMParser();
            var result = parser.ParseTrigger("""TRIGGER power_on "turn power on";""");
            Assert.Equal(typeof(ParsedTrigger), result.GetType());
            Assert.Equal("power_on", result.Identifier);
            Assert.Equal("turn power on", result.Description);
        }

        [Fact]
        public void ParsesActionAndReturnsTuple()
        {
            var parser = new FSMParser();
            var result = parser.ParseAction("""ACTION on "Turn lamp on" : ENTRY_ACTION;""");
            Assert.Equal(typeof(ParsedAction), result.GetType());
            Assert.Equal("on", result.Identifier);
            Assert.Equal("Turn lamp on", result.Description);
            Assert.Equal("ENTRY_ACTION", result.Type.ToString());
        }
    }
}



