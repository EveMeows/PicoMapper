using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PicoMapper;

public class Mapper : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch = null!;

    public Mapper()
    {
        this.graphics = new GraphicsDeviceManager(this);
        this.Content.RootDirectory = "Content";
        
        this.IsMouseVisible = true;
        this.Window.AllowUserResizing = true;
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        this.GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
    }
}
