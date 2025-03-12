namespace PicoMapper;

public static class PicoMapper
{
    [STAThread]
    public static void Main()
    { 
        using var game = new Mapper();
        game.Run();
    }
}