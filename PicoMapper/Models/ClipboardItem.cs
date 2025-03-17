using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace PicoMapper.Models;

public class ClipboardItem
{
    public required int[,] Buffer { get; set; }
    public required Rectangle Selection { get; set; }
}
