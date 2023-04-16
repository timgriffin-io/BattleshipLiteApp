namespace BattleshipLiteData.Models;

public class GridLocation
{
    public string Row { get; set; }
    public int Column { get; set; }
    public GridLocationStatus Status { get; set; } = GridLocationStatus.Empty;
}
