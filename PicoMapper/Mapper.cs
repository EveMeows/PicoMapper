﻿using Microsoft.Xna.Framework;
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
    public bool IsResized { get; private set; }

    private void SizeChanged(object? sender, EventArgs e)
    {
        this.IsResized = true;
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
        this.Context.SwitchState(new Editor(this));
    }

    protected override void Update(GameTime gameTime)
    {
        InputHelper.GetState();

        this.Context.Update(gameTime);
        base.Update(gameTime);

        if (this.IsResized) this.IsResized = false;
    }

    protected override void Draw(GameTime gameTime)
    {
        this.Context.Draw(gameTime, this.spriteBatch);
        base.Draw(gameTime);
    }
}
