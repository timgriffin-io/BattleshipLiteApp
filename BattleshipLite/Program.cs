using BattleshipLite;
using BattleshipLiteData.Models;
using BattleshipLiteLogic;


ConsoleUI.WelcomeMessage();
ConsoleUI.DisplayGameRules();

bool playAgain = true;

while(playAgain == true)
{

    Player activePlayer = ConsoleUI.CreatePlayer("Player 1");
    Player opponent = ConsoleUI.CreatePlayer("Player 2");
    Player winner = null;

    while (winner == null)
    {
        ConsoleUI.DisplayShotGrid(activePlayer);

        ConsoleUI.RecordPlayerShot(activePlayer, opponent);

        bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
        if (doesGameContinue == true)
        {
            //swap positions using tuples
            (activePlayer, opponent) = (opponent, activePlayer);
        }
        else
        {
            winner = activePlayer;
            ConsoleUI.ShowWinner(winner);
        }
    }

    playAgain = ConsoleUI.PlayAgain();
    ConsoleUI.ClearGame();
}

ConsoleUI.EndGame();
