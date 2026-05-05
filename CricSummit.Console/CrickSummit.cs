using CricSummit.Console.Challenges;
using CricSummit.Console.Interfaces;
using CricSummit.Domain.ValueObjects;

namespace CricSummit.Console
{
    public class CrickSummit
    {
        private readonly IInputProvider _inputProvider;
        private readonly PredictOutcomeHandler _predictOutcomeHandler;
        private readonly CommentaryHandler _commentaryHandler;
        private readonly SuperOverHandler _superOverHandler;

        public CrickSummit(
            IInputProvider inputProvider,
            PredictOutcomeHandler predictOutcomeHandler,
            CommentaryHandler commentaryHandler,
            SuperOverHandler superOverHandler
        )
        {
            _inputProvider = inputProvider;
            _predictOutcomeHandler = predictOutcomeHandler;
            _commentaryHandler = commentaryHandler;
            _superOverHandler = superOverHandler;
        }

        public void Run()
        {
            while (true)
            {
                if (_inputProvider.IsInteractive)
                {
                    RunAvailableEnums();
                    System.Console.WriteLine("Select a challenge:");
                    System.Console.WriteLine("1. Predict Outcome");
                    System.Console.WriteLine("2. Commentary");
                    System.Console.WriteLine("3. Super Over\n");
                }
                var choice = _inputProvider.ReadLine().Trim();
                switch (choice)
                {
                    case "1":
                        _predictOutcomeHandler.Execute();
                        break;
                    case "2":
                        _commentaryHandler.Execute();
                        break;
                    case "3":
                        _superOverHandler.Execute();
                        break;
                    default:
                        System.Console.WriteLine("Invalid choice \n");
                        break;
                }
            }
        }

        private void RunAvailableEnums()
        {
            System
                .Console
                .WriteLine(
                    $"\n Available bowling types: {string.Join(", ", Enum.GetNames<BowlingType>())}"
                );

            System
                .Console
                .WriteLine(
                    $"Available batting types: {string.Join(", ", Enum.GetNames<BattingType>())}"
                );

            System
                .Console
                .WriteLine(
                    $"Available shot timings: {string.Join(", ", Enum.GetNames<ShotTiming>())}\n"
                );
        }
    }
}
