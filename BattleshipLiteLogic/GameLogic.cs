using BattleshipLiteData;
using BattleshipLiteData.Models;

namespace BattleshipLiteLogic;

public static class GameLogic
{

    private static readonly List<string> _letters = new() { "A", "B", "C", "D", "E" };
    private static readonly List<int> _numbers = new() { 1, 2, 3, 4, 5 };

    public static void InitializeShotGrid(Player player)
    {
        foreach (var letter in _letters) 
        {
            foreach (var number in _numbers)
            {
                AddShot(player, letter, number);
            }
        }
    }

    public static bool PlaceShip(Player player, string ship)
    {
        (string row, int col) = SplitInputIntoRowAndColumn(ship);

        bool isValidLocation = ValidateGridLocation(player, row, col);
        bool isLocationOpen = ValidateShipLocation(player, row, col);

        if(isValidLocation && isLocationOpen)
        {
            AddShip(player, row, col);
            return true;
        }
        return false;
    }

    public static bool PlayerStillActive(Player player)
        => player.ShipLocations.Count(ship => ship.Status == GridLocationStatus.Sunk) < 5;
    
    public static int GetShotCount(Player player)
        => player.ShotGrid.Count(shot => shot.Status == GridLocationStatus.Hit 
                                   || shot.Status == GridLocationStatus.Miss);

    public static (string row, int col) SplitInputIntoRowAndColumn(string input)
    {
        if(input.Length != 2)
        {
            throw new ArgumentException();
        }

        var inputArray = input.ToArray();
        string row = inputArray[0].ToString().ToUpper();
        int col = int.Parse(inputArray[1].ToString());

        return (row, col);
    }

    public static bool ValidateShot(Player player, string row, int col)
        => player.ShotGrid.Any(shot => shot.Row == row && shot.Column == col && shot.Status == GridLocationStatus.Empty);

    public static bool IdentifyShotResult(Player player, ShotResult shotResult)
        => IsShipLocation(player, shotResult.Row, shotResult.Column);
    
    public static void RecordShotResult(Player activePlayer, Player opponent, ShotResult shotResult, bool isAHit)
    {
        var shot = activePlayer.ShotGrid.First(shot => shot.Row == shotResult.Row && shot.Column == shotResult.Column);
        if (isAHit)
        {
            shot.Status = GridLocationStatus.Hit;
            opponent.ShipLocations.First(ship => ship.Row == shotResult.Row && ship.Column == shot.Column).Status = GridLocationStatus.Sunk;
        }
        else
        {
            shot.Status = GridLocationStatus.Miss;
        }
    }

    private static void AddShot(Player player, string letter, int col)
    {
        var shot = new GridLocation
        {
            Row = letter,
            Column = col,
            Status = GridLocationStatus.Empty
        };

        player.ShotGrid.Add(shot);
    }

    private static void AddShip(Player player, string row, int col)
    {
        var ship = new GridLocation()
        {
            Row = row.ToUpper(),
            Column = col,
            Status = GridLocationStatus.Ship,
        };

        player.ShipLocations.Add(ship);
    }

    private static bool ValidateShipLocation(Player player, string row, int col)
        => IsShipLocation(player, row, col) == false;

    private static bool ValidateGridLocation(Player player, string row, int col)
        => player.ShotGrid.Any(shot => shot.Row == row && shot.Column == col);

    private static bool IsShipLocation(Player player, string row, int col)
        => player.ShipLocations.Any(ship => ship.Row == row && ship.Column == col);


}
