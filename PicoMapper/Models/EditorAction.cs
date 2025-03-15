namespace PicoMapper.Models;

public class EditorAction
{
    public int X { get; set; }
    public int Y { get; set; }

    public int Layer { get; set; }

    public int PreviousTile { get; set; }
    public int NextTile { get; set; }
}
