////using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.UI;

using Color = Microsoft.Xna.Framework.Color;
using Button = MonoGayme.Core.UI.Button;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Microsoft.Xna.Framework;

namespace PicoMapper.UI;

public class Menu : Component, IUpdateableComponent, IDrawableComponent
{
    public readonly UIController Buttons = new UIController(false);
    public readonly Rectangle Size;

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

    public Menu(Rectangle size, Mapper window) 
    {
        this.Size = size;

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);
    }

    public void Update(GameTime time)
    {
        this.Buttons.Update(InputHelper.GetMousePosition());
    }
    
    public void Draw(SpriteBatch batch, Camera2D? camera = null)
    {
        batch.Draw(
            this.pixel,
            this.Size,
            Colours.Crimson
        );

        this.DrawRectangleLines(this.Size, Colours.BrightRed, batch);

        this.Buttons.Draw(batch, camera);
    }
}
