using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace PicoMapper.Models;

public class Tile
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("texture")]
    public required Texture2D Texture { get; set; }
}
