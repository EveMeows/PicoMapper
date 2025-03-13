using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.UI;
using MonoGayme.Core.Utilities;
using PicoMapper.States;
using Button = MonoGayme.Core.UI.Button;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace PicoMapper.Components;

public class TileViewer : Component, IDrawableComponent, IUpdateableComponent
{
    private readonly Mapper window;
    private readonly Editor editor;

    private readonly int Scale = 3;
    private readonly UIController ui = new UIController(false);

    private bool handleOpen = false;
    private readonly Texture2D handleTexture;
    private readonly TextureButton handle;

    private readonly Vector2 ViewSize = new Vector2(32);
    private readonly int ViewCount = 5;

    private Rectangle viewRect;

    private readonly int BorderThick = 3;

    private readonly Texture2D pixel;

    public void DrawRectangleLines(float x, float y, float width, float height, Color colour, SpriteBatch batch, float alpha = 1)
    {
        // Top
        batch.Draw(this.pixel, new Rectangle((int)x, (int)y, (int)width, 1), colour * alpha);

        // Left
        batch.Draw(this.pixel, new Rectangle((int)x, (int)y, 1, (int)height), colour * alpha);

        // Right
        batch.Draw(this.pixel, new Rectangle((int)(x + width - 1), (int)y, 1, (int)height), colour * alpha);

        // Bottom
        batch.Draw(this.pixel, new Rectangle((int)x, (int)(y + height - 1), (int)width, 1), colour * alpha);
    }

    public void DrawRectangleLines(Rectangle rect, Color colour, SpriteBatch batch)
        => this.DrawRectangleLines(rect.X, rect.Y, rect.Width, rect.Height, colour, batch);

    private void Handle(Button self)
    {
        this.handleOpen = !this.handleOpen;
    }

    public TileViewer(Mapper window, Editor editor)
    {
        this.window = window;
        this.editor = editor;

        this.handleTexture = this.window.Content.Load<Texture2D>("UI/Handle");
        this.handle = new TextureButton(
            this.handleTexture,
            new Vector2(this.window.GameSize.X - this.handleTexture.Width * this.Scale, 30),
            Color.White,
            this.Scale
        ) { OnClick = this.Handle };

        this.ui.Add(this.handle);

        this.viewRect = new Rectangle(
            (int)(this.window.GameSize.X - this.ViewCount * this.ViewSize.X),
            20, (int)(this.ViewCount * this.ViewSize.X), (int)(this.window.GameSize.Y - 90)
        );

        this.pixel = new Texture2D(this.window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);
    }

    public void Update(GameTime time)
    {
        if (!this.handleOpen)
        {
            this.handle.SetPosition(new Vector2(this.window.GameSize.X - this.handleTexture.Width * this.Scale, 30));
        }
        else
        {
            this.handle.SetPosition(new Vector2(this.viewRect.X - this.handleTexture.Width * this.Scale - this.BorderThick, 30));
        }

        this.viewRect = new Rectangle(
            (int)(this.window.GameSize.X - this.ViewCount * this.ViewSize.X),
            20, (int)(this.ViewCount * this.ViewSize.X), (int)(this.window.GameSize.Y - 90)
        );

        int x = 0;
        int y = 0;
        foreach (KeyValuePair<int, Texture2D> tile in this.editor.TileCache)
        {
            // Calculate scale
            Vector2 scale = this.ViewSize / new Vector2(tile.Value.Width, tile.Value.Height);

            if (
                Collision.CheckRectPoint(
                    InputHelper.GetMousePosition(), 
                    new Rectangle(
                        this.viewRect.X + x * (int)this.ViewSize.X, 
                        this.viewRect.Y + y * (int)this.ViewSize.Y, 
                        (int)this.ViewSize.X,  (int)this.ViewSize.Y
                    )
                )
            )
            {
                if (InputHelper.IsMousePressed(MouseButton.Left))
                {
                    this.editor.ActiveTile = tile.Key;
                }
            }

            x++;
            if (x > this.ViewCount - 1)
            {
                x = 0;
                y++;
            }
        }


        this.ui.Update(InputHelper.GetMousePosition());
    }

    public void Draw(SpriteBatch batch, Camera2D? camera = null)
    {
        this.ui.Draw(batch, camera);
        if (this.handleOpen)
        {
            batch.Draw(this.pixel, this.viewRect, new Color(26, 26, 26));
            batch.Draw(this.pixel, new Rectangle(this.viewRect.X - this.BorderThick, this.viewRect.Y, this.BorderThick, this.viewRect.Height), Colours.Crimson);

            int x = 0;
            int y = 0;
            foreach (KeyValuePair<int, Texture2D> tile in this.editor.TileCache)
            {
                // Calculate scale
                Vector2 scale = this.ViewSize / new Vector2(tile.Value.Width, tile.Value.Height);
                batch.Draw(tile.Value, new Vector2(this.viewRect.X + x * this.ViewSize.X, this.viewRect.Y + y * this.ViewSize.Y), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

                // Draw active
                if (this.editor.ActiveTile == tile.Key)
                {
                    this.DrawRectangleLines(this.viewRect.X + x * this.ViewSize.X, this.viewRect.Y + y * this.ViewSize.Y, this.ViewSize.X, this.ViewSize.Y, Color.White, batch);
                }

                x++;
                if (x > this.ViewCount - 1)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }
}
