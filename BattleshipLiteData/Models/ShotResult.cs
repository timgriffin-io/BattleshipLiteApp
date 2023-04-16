namespace BattleshipLiteData.Models
{
    public class ShotResult
    {
        public string Row { get; set; } = string.Empty;
        public int Column { get; set; } = 0;
        public bool IsValidShot { get; set; } = false;
    }
}
