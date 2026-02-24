using CricSummit.Console.Interfaces;

namespace CricSummit.Console.InputProviders
{
    public class ConsoleInputProvider : IInputProvider
    {
        public bool IsInteractive => true;

        public string ReadLine()
        {
            var result = System.Console.ReadLine();
            return result ?? string.Empty;
        }

        public IEnumerable<string[]> ReadAll()
        {
            var result = System.Console.ReadLine();
            if (result == null)
                return [ ];
            return [ .. result.Split(' ').Select(input => input.Split("_")) ];
        }
    }
}
