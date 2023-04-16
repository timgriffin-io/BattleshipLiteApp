namespace BattleshipLiteData.Models;

public class Player
{
    public string PlayerName { get; set; }
	public List<GridLocation> ShipLocations { get; set; } = new List<GridLocation>();
	public List<GridLocation> ShotGrid { get; set; } = new List<GridLocation>();
}
