namespace PicoMapper;

public static class PicoMapper
{
    [STAThread]
    public static void Main()
    {
        // Make the UI look good.
        Application.EnableVisualStyles();

        using var game = new Mapper();
        game.Run();
    }
}