using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Input;
using MonoGayme.Core.States;
using PicoMapper.States;

namespace PicoMapper;

public class Mapper : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch = null!;

    public StateContext Context { get; } = new StateContext();
    public Vector2 GameSize { get; private set; }

    public string Version { get; } = "0.6.2";

    private void SizeChanged(object? sender, EventArgs e)
    {
        this.GameSize = new Vector2(
            this.Window.ClientBounds.Width, 
            this.Window.ClientBounds.Height
        );
    }

    public Mapper()
    {
        this.graphics = new GraphicsDeviceManager(this);
        this.Content.RootDirectory = "Content";
        
        this.IsMouseVisible = true;
        this.Window.AllowUserResizing = true;

        this.Window.ClientSizeChanged += this.SizeChanged;
        this.GameSize = new Vector2(
            this.Window.ClientBounds.Width, 
            this.Window.ClientBounds.Height
        );
    }

    protected override void LoadContent()
    {
        this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
        // this.Context.SwitchState(new Editor(this, new Models.Map { TileX = 8, TileY = 8, GridX = 10, GridY = 10, Layers = [], Tiles = [] }));
        this.Context.SwitchState(new MainMenu(this));
    }

    protected override void Update(GameTime gameTime)
    {
        InputHelper.GetState();

        if (this.IsActive)
        {
            this.Context.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        this.Context.Draw(gameTime, this.spriteBatch);
        base.Draw(gameTime);
    }
}
