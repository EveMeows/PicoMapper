using System.Text.Json.Serialization;

namespace PicoMapper.Models;

public class Map
{
    [JsonPropertyName("GridX")]
    public int GridX { get; set; }

    [JsonPropertyName("GridY")]
    public int GridY { get; set; }


    [JsonPropertyName("TileX")]
    public int TileX { get; set; }

    [JsonPropertyName("TileY")]
    public int TileY { get; set; }


    [JsonPropertyName("Layers")]
    public List<int[,]> Layers { get; set; } = null!;
}
