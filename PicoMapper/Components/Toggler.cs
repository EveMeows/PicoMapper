using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Input;
using MonoGayme.Core.UI;
using PicoMapper.UI;

namespace PicoMapper.Components;

public class Toggler : Component, IDrawableComponent, IUpdateableComponent
{
    public Color BGColour = Colours.Grey;
    public int BGSize = 40;

    public Color InfoColour = Colours.BrightRed;
    public Color InfoTextColour = Colours.Crimson;
    public int InfoSize = 30;

    private readonly Texture2D pixel;
    private readonly Mapper window;

    public Selected Active = Selected.Pencil;
    private readonly IReadOnlyList<ToggleButton> buttons = [];

    private readonly SpriteFont font;
    public string ToolTip = "";

    private float uiY;
    private readonly float Scale;
    private readonly float SpriteSize;

    private void OnToggle(Button self)
    {
        if (self is ToggleButton btn)
        {
            foreach (ToggleButton others in this.buttons)
                others.Colour = Color.DarkGray;

            btn.Colour = Color.White;
            this.Active = btn.Type;
        }
    }

    private void OnHover(Button self)
    {
        if (self is ToggleButton btn)
        {
            this.ToolTip = $"{btn.Type} Tool ({btn.Keybind})";
        }
    }

    private void OnExit(Button self)
        => this.ToolTip = "";

    private ToggleButton CreateButton(Selected type, string path, float x, Keys keybind)
    {
        return new ToggleButton(
            type, this.window.Content.Load<Texture2D>(path),
            this.Scale, new Vector2(x, this.uiY), keybind
        ) { OnClick = this.OnToggle, OnHover = this.OnHover, OnMouseExit = this.OnExit };
    }

    public Toggler(Mapper window)
    {
        this.uiY = window.GameSize.Y - this.BGSize + 4;

        this.Scale = 2;
        this.SpriteSize = 16 * 2;

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.window = window;

        float offset = 5;
        this.buttons = [
            this.CreateButton(Selected.Pencil, "UI/Buttons/Pencil", offset, Keys.D),
            this.CreateButton(Selected.Eraser, "UI/Buttons/Eraser", this.SpriteSize + offset * 2, Keys.E),
            this.CreateButton(Selected.Bucket, "UI/Buttons/Bucket", this.SpriteSize * 2 + offset * 3, Keys.B),
            this.CreateButton(Selected.Select, "UI/Buttons/Move", this.SpriteSize * 3 + offset * 4, Keys.S),
            this.CreateButton(Selected.Move, "UI/Buttons/Hand", this.SpriteSize * 4 + offset * 5, Keys.Space),
        ];

        this.buttons[0].Colour = Color.White;

        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoNormal");
    }

    public void Update(GameTime time)
    {
        foreach (ToggleButton btn in this.buttons)
        {
            btn.Update(InputHelper.GetMousePosition());

            if (InputHelper.IsKeyPressed(btn.Keybind))
            { 
                foreach (ToggleButton others in this.buttons)
                    others.Colour = Color.DarkGray;

                btn.Colour = Color.White;
                this.Active = btn.Type;
            }
        }
    }

    public void Draw(SpriteBatch batch, Camera2D? camera = null)
    {
        // Background band
        batch.Draw(
            this.pixel,
            new Rectangle(
                0, (int)this.window.GameSize.Y - this.BGSize, 
                (int)this.window.GameSize.X, this.BGSize
            ),
            this.BGColour
        );

        // Info Band
        batch.Draw(
            this.pixel,
            new Rectangle(
                0, (int)this.window.GameSize.Y - this.BGSize - this.InfoSize,
                (int)this.window.GameSize.X, this.InfoSize
            ),
            this.InfoColour
        );

        batch.DrawString(this.font, this.ToolTip, new Vector2(5, (int)this.window.GameSize.Y - this.BGSize - this.InfoSize + 4), this.InfoTextColour);

        foreach (ToggleButton btn in this.buttons)
        {
            // In case the game size updates...
            this.uiY = this.window.GameSize.Y - this.BGSize + 4;
            btn.SetPositionY(this.uiY);

            btn.Draw(batch, camera);
        }
    }
}
