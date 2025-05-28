using DPAT.Domain;

namespace DPAT.Presentation
{
    public class ConsoleRenderer : IRenderer
    {
        private const char TOP_LEFT = '┌';
        private const char TOP_RIGHT = '┐';
        private const char BOTTOM_LEFT = '└';
        private const char BOTTOM_RIGHT = '┘';
        private const char HORIZONTAL = '─';
        private const char VERTICAL = '│';
        private const char EMPTY = ' ';
        private const int REFRESH_INTERVAL_MS = 100;
        private const string TITLE = "Finite State Machine Diagram";

        private int lastWidth;
        private int lastHeight;
        private bool isRunning;

        public void Render(FSM fsm)
        {
            isRunning = true;
            lastWidth = Console.LargestWindowWidth;
            lastHeight = Console.LargestWindowHeight;

            DrawBox();
            Console.CursorVisible = false;
            while (isRunning)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Stop();
                        break;
                    }
                }

                if (HasConsoleSizeChanged())
                {
                    Console.Clear();
                    DrawBox();
                }
                Thread.Sleep(REFRESH_INTERVAL_MS);
            }
        }

        public void Stop()
        {
            isRunning = false;
            Console.CursorVisible = true;
        }

        private bool HasConsoleSizeChanged()
        {
            int currentWidth = Console.LargestWindowWidth;
            int currentHeight = Console.LargestWindowHeight;

            if (currentWidth != lastWidth || currentHeight != lastHeight)
            {
                lastWidth = currentWidth;
                lastHeight = currentHeight;
                return true;
            }
            return false;
        }

        private void DrawBox()
        {
            DrawTopBorder();
            DrawSides();
            DrawBottomBorder();
        }

        private void DrawTopBorder()
        {
            Console.Write(TOP_LEFT);

            int totalWidth = Console.LargestWindowWidth - 2;
            int titleStart = (totalWidth - TITLE.Length) / 2;

            // Draw left side of border
            for (int i = 0; i < titleStart - 2; i++)
            {
                Console.Write(HORIZONTAL);
            }

            // Draw title
            Console.Write("= ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(TITLE);
            Console.ResetColor();
            Console.Write(" =");


            // Draw right side of border
            for (int i = titleStart + TITLE.Length; i < totalWidth - 2; i++)
            {
                Console.Write(HORIZONTAL);
            }

            Console.Write(TOP_RIGHT);
        }

        private void DrawBottomBorder()
        {
            Console.Write(BOTTOM_LEFT);
            DrawHorizontalLine();
            Console.Write(BOTTOM_RIGHT);
        }

        private void DrawSides()
        {
            for (int i = 0; i < Console.LargestWindowHeight - 2; i++)
            {
                Console.Write(VERTICAL);
                DrawContent();
                Console.Write(VERTICAL);
            }
        }

        private void DrawHorizontalLine()
        {
            for (int i = 0; i < Console.LargestWindowWidth - 2; i++)
            {
                Console.Write(HORIZONTAL);
            }
        }

        private void DrawContent()
        {
            // Draw empty space for now
            for (int j = 0; j < Console.LargestWindowWidth - 2; j++)
            {
                Console.Write(EMPTY);
            }
        }
    }
}