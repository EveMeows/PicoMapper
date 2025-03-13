using System.Text.Json;
using System.Text.Json.Serialization;

namespace PicoMapper.Models;

public class LayerConverter : JsonConverter<List<int[,]>>
{
    public override List<int[,]> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = new List<int[,]>();
        var jsonArray = JsonSerializer.Deserialize<List<int[][]>>(ref reader, options);

        if (jsonArray != null)
        {
            foreach (var jaggedArray in jsonArray)
            {
                int rows = jaggedArray.Length;
                int cols = jaggedArray[0].Length;
                int[,] multiDimArray = new int[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        multiDimArray[i, j] = jaggedArray[i][j];
                    }
                }

                list.Add(multiDimArray);
            }
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<int[,]> value, JsonSerializerOptions options)
    {
        var convertedList = new List<int[][]>();

        foreach (var matrix in value)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[][] jaggedArray = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = matrix[i, j];
                }
            }

            convertedList.Add(jaggedArray);
        }

        JsonSerializer.Serialize(writer, convertedList, options);
    }
}