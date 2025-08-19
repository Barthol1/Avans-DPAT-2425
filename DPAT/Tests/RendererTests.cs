using System;
using System.IO;
using System.Text;
using DPAT.Infrastructure;
using DPAT.Presentation;
using Xunit;

namespace DPAT.Tests
{
    public class RendererTests
    {
        private static string ResolvePath(string relative)
        {
            var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            return Path.Combine(root, "fsm_files", relative);
        }

        [Fact]
        public void Print_Renders_Text_With_Headings()
        {
            var director = new FSMDirector(new FSMBuilder());
            var fsm = director.BuildFromFile(ResolvePath("example_lamp.fsm"));

            var sb = new StringBuilder();
            using var writer = new StringWriter(sb);
            var originalOut = Console.Out;
            Console.SetOut(writer);

            try
            {
                var renderer = new ConsoleRenderer();
                renderer.Render(fsm);
            }
            finally
            {
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            }

            var output = sb.ToString();
            Assert.Contains("FSM Visualization:", output);
            Assert.Contains("Transition Summary:", output);
        }
    }
}



