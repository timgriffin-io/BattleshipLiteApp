using BattleshipLiteData;
using BattleshipLiteData.Models;
using BattleshipLiteLogic;

namespace BattleshipLite;

internal static class ConsoleUI
{
    internal static void WelcomeMessage()
    {
        Console.WriteLine("Welcome to Battleship Lite");
        Console.WriteLine("created by Tim Griffin");
        Console.WriteLine();
    }

    internal static void DisplayGameRules()
    {
        Console.WriteLine("Game Rules");
        Console.WriteLine("1. Each ship only occupies 1 grid location.");
        Console.WriteLine("2. The first player to sink 5 ships wins the game.");
        Console.WriteLine("3. Entering 'uncle' at any time ends the game...");
        Console.WriteLine();
        Console.WriteLine("Playble grid locations:");
        DisplayGridLocations();
    }

    internal static Player CreatePlayer(string defaultName)
    {
        var player = new Player();

        SetPlayerName(player, defaultName);
        GameLogic.InitializeShotGrid(player);
        PlacePlayerShips(player);
        Console.Clear();

        return player;
    }

    internal static void DisplayShotGrid(Player player, bool isDummy = false)
    {
        if(isDummy == false)
        {
            Console.WriteLine($"{player.PlayerName}'s Shot Grid:");
        }
        var currentRow = player.ShotGrid[0].Row;

        foreach (var shot in player.ShotGrid)
        {
            if(shot.Row != currentRow)
            {
                Console.WriteLine();
                currentRow = shot.Row;
            }

            DisplayShotGridLocation(shot);
        }

        Console.WriteLine();
        Console.WriteLine();
    }

    internal static void RecordPlayerShot(Player activePlayer, Player opponent)
    {
        var shotResult = new ShotResult();

        while (shotResult.IsValidShot == false)
        {
            var shot = AskForShot(activePlayer.PlayerName);
            CheckForUncle(shot, activePlayer);
            shotResult = HandleShot(activePlayer, shot);
        }

        bool isAHit = GameLogic.IdentifyShotResult(opponent, shotResult);
        GameLogic.RecordShotResult(activePlayer, opponent, shotResult, isAHit);
        DisplayHitOrMiss(isAHit);
    }

    internal static void ShowWinner(Player winner)
    {
        Console.WriteLine($"Congratulations, {winner.PlayerName}.  You won!");
        Console.WriteLine($"{winner.PlayerName} took {GameLogic.GetShotCount(winner)} shots.");
        Console.WriteLine();
    }

    internal static bool PlayAgain()
    {
        Console.WriteLine($"Start a new game?");
        Console.Write($"Enter 'yes' to continue: ");
        var input = Console.ReadLine();
        return input.ToUpper() == "YES";
    }

    internal static void ClearGame()
    {
        Console.Clear();
    }

    internal static void EndGame() 
    {
        Console.WriteLine();
        Console.WriteLine("Thank you for playing!");
    }

    private static void CheckForUncle(string input, Player activePlayer)
    {
        if (input.ToUpper() == "UNCLE")
        {
            var defaulColor = Console.ForegroundColor;
            Console.Clear();
            Console.WriteLine($"{activePlayer.PlayerName} said uncle...LOL", Console.ForegroundColor = ConsoleColor.Magenta);
            Console.ForegroundColor = defaulColor;
            Console.WriteLine();
            Environment.Exit(0);
        }
    }

    private static void DisplayGridLocations()
    {
        var dummy = new Player();
        GameLogic.InitializeShotGrid(dummy);
        DisplayShotGrid(dummy, true);
        Console.WriteLine();
    }

    private static void SetPlayerName(Player player, string defaultName)
    {
        Console.WriteLine($"Player information for {defaultName}");
        var playerName = GetUserName();
        player.PlayerName = playerName == string.Empty ? defaultName : playerName;
        CheckForUncle(playerName, player);
    }

    private static string GetUserName()
    {
        Console.Write("Enter your name: ");
        return Console.ReadLine();
    }

    private static void DirectOpponentToLookAway()
    {
        Console.WriteLine($"Opponent, please look away...");
        Console.WriteLine();
    }

    private static void PlacePlayerShips(Player player)
    {
        DirectOpponentToLookAway();

        while (player.ShipLocations.Count < 5)
        {
            Console.Write($"{player.PlayerName}, enter location for ship number {player.ShipLocations.Count + 1}: ");
            string ship = Console.ReadLine();
            CheckForUncle(ship, player);       
            HandleShipPlacement(player, ship);
        }
    }

    private static void DisplayShotGridLocation(GridLocation shot)
    {
        var defaultColor = Console.ForegroundColor;
        if (shot.Status == GridLocationStatus.Empty)
        {
            Console.Write($"{shot.Row}{shot.Column} ", Console.ForegroundColor = ConsoleColor.Green);
        }
        else if (shot.Status == GridLocationStatus.Miss)
        {
            Console.Write($" O ", Console.ForegroundColor = ConsoleColor.Yellow);
        }
        else if (shot.Status == GridLocationStatus.Hit)
        {
            Console.Write($" X ", Console.ForegroundColor = ConsoleColor.Red);
        }
        Console.ForegroundColor = defaultColor;
    }

    private static void HandleShipPlacement(Player player, string ship)
    {
        bool isValidLocation;
        try
        {
            isValidLocation = GameLogic.PlaceShip(player, ship);
        }
        catch (Exception)
        {
            isValidLocation = false;
        }

        if (isValidLocation == false)
        {
            Console.WriteLine($"Invalid ship location.  Please try again.");
        }
    }

    private static ShotResult HandleShot(Player player, string shot)
    {
        string row = "";
        int col = 0;
        bool isValidShot;

        try
        {
            (row, col) = GameLogic.SplitInputIntoRowAndColumn(shot);
            isValidShot = GameLogic.ValidateShot(player, row, col);
        }
        catch (Exception)
        {
            isValidShot = false;
        }

        if (isValidShot == false)
        {
            Console.WriteLine(value: "Invalid shot location.  Please try again.");
        }

        return new ShotResult() { Row = row, Column = col, IsValidShot = isValidShot};
    }

    private static string AskForShot(string playerName)
    {
        Console.Write($"{playerName}, place your shot: ");
        return Console.ReadLine();
    }

    private static void DisplayHitOrMiss(bool isAHit)
    {
        var defaultColor = Console.ForegroundColor;
        if (isAHit)
        {
            Console.WriteLine(format: "HIT!", Console.ForegroundColor = ConsoleColor.Red);
        }
        else
        {
            Console.WriteLine(format: "Miss...", Console.ForegroundColor = ConsoleColor.Yellow);
        }
        Console.ForegroundColor = defaultColor;
        Console.WriteLine();
        Console.WriteLine("------------------------------------------");
        Console.WriteLine();
    }

}
