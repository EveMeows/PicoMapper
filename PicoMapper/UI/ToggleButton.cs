using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Components;
using MonoGayme.Core.UI;
using MonoGayme.Core.Utilities;
using PicoMapper.Components;

using Color = Microsoft.Xna.Framework.Color;
using Button = MonoGayme.Core.UI.Button;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace PicoMapper.UI;

public class ToggleButton : Button
{
    private Rectangle rect;
    private bool holding = false;
    private bool entered = false;

    private readonly float scale;
    private readonly Texture2D texture;

    public Selected Type;

    public Keys Keybind;

    public ToggleButton(Selected type, Texture2D texture, float scale, Vector2 position, Keys keybind) : base(false)
    {
        this.texture = texture;
        this.Position = position;
        this.scale = scale;

        this.rect = new Rectangle(
            (int)position.X, (int)position.Y,
            (int)(texture.Width * scale), (int)(texture.Height * scale)
        );

        this.Colour = Color.DarkGray;

        this.Type = type;
        this.Keybind = keybind;
    }

    public void SetPositionY(float @new)
    {
        this.Position = this.Position with { Y = @new };
        this.rect.Y = (int)@new;
    }

    public override void Draw(SpriteBatch batch, Camera2D? camera)
    {
        batch.Draw(this.texture, this.Position, null, this.Colour, 0, Vector2.Zero, this.scale, SpriteEffects.None, 0);
    }

    public override void Update(Vector2 mouse)
    {
        if (Collision.CheckRectPoint(mouse, this.rect))
        {
            this.OnHover?.Invoke(this);

            if (!this.entered)
            {
                this.OnMouseEnter?.Invoke(this);
                this.entered = true;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                this.holding = true;

            if (Mouse.GetState().LeftButton != ButtonState.Released || !this.holding) return;

            this.RunAction();
            this.holding = false;
        }
        else
        {
            if (this.entered)
            {
                this.OnMouseExit?.Invoke(this);
            }

            this.holding = false;
            this.entered = false;
        }
    }
}
