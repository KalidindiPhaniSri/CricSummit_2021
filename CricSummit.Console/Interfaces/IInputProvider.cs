namespace CricSummit.Console.Interfaces
{
    public interface IInputProvider
    {
        bool IsInteractive { get; }
        string ReadLine();
        IEnumerable<string[]> ReadAll();
    }
}
