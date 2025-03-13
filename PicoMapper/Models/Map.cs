using System.Text.Json.Serialization;

namespace PicoMapper.Models;

public class Map
{
    [JsonPropertyName("grid_x")]
    public int GridX { get; set; }

    [JsonPropertyName("grid_y")]
    public int GridY { get; set; }


    [JsonPropertyName("tile_x")]
    public int TileX { get; set; }

    [JsonPropertyName("tile_y")]
    public int TileY { get; set; }


    [JsonPropertyName("tiles")]
    public List<Tile> Tiles { get; set; } = null!;

    [JsonPropertyName("layers")]
    public List<int[,]> Layers { get; set; } = null!;
}
