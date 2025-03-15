using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Input;

using Color = Microsoft.Xna.Framework.Color;
using Button = MonoGayme.Core.UI.Button;
using ToggleButton = PicoMapper.UI.ToggleButton;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using PicoMapper.States;
using MonoGayme.Core.Utilities;

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

    private readonly TileViewer? viewer;

    private readonly float Offset = 5;

    private readonly Editor editor;

    private Rectangle layerRect = new Rectangle();
    private bool layerEnter = false;

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

    public Toggler(Mapper window, Editor editor)
    {
        this.uiY = window.GameSize.Y - this.BGSize + 4;

        this.Scale = 2;
        this.SpriteSize = 16 * 2;

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.window = window;

        this.buttons = [
            this.CreateButton(Selected.Pencil, "UI/Buttons/Pencil", this.Offset, Keys.D),
            this.CreateButton(Selected.Eraser, "UI/Buttons/Eraser", this.SpriteSize + this.Offset * 2, Keys.E),
            this.CreateButton(Selected.Bucket, "UI/Buttons/Bucket", this.SpriteSize * 2 + this.Offset * 3, Keys.B),
            this.CreateButton(Selected.Select, "UI/Buttons/Move", this.SpriteSize * 3 + this.Offset * 4, Keys.S),
            this.CreateButton(Selected.Move, "UI/Buttons/Hand", this.SpriteSize * 4 + this.Offset * 5, Keys.Space),
        ];

        this.buttons[0].Colour = Color.White;

        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoNormal");

        this.editor = editor;
        this.viewer = editor.NormalComponents.Get<TileViewer>();
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

        if (Collision.CheckRectPoint(InputHelper.GetMousePosition(), this.layerRect))
        {
            this.layerEnter = true;
            this.ToolTip = "Layer (Left = next, Right = previous)";

            if (InputHelper.IsMousePressed(MouseButton.Left))
            {
                this.editor.ActiveLayer++;
            }

            if (InputHelper.IsMousePressed(MouseButton.Right))
            {
                this.editor.ActiveLayer--;
                if (this.editor.ActiveLayer < 0)
                {
                    this.editor.ActiveLayer = 0;
                }
            }
        }
        else
        {
            if (this.layerEnter)
            {
                this.ToolTip = "";
                this.layerEnter = false;
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

        // Draw active tile
        if (this.viewer is null) return;

        int baseX = (int)(this.SpriteSize * 5 + this.Offset * 7);

        // Background
        batch.Draw(
            this.pixel,
            new Rectangle(baseX, (int)this.uiY, (int)this.viewer.ViewSize.X, (int)this.viewer.ViewSize.Y),
            Color.Black
        );

        // Tile
        if (this.editor.ActiveTile != 0)
        {
            if (this.editor.TileCache.TryGetValue(this.editor.ActiveTile, out Texture2D? texture))
            {
                // Scale
                Vector2 scale = this.viewer.ViewSize / new Vector2(texture.Width, texture.Height);

                batch.Draw(
                    texture, new Vector2(baseX, (int)this.uiY), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0
                );       
            }
        }

        int offset = 4;

        // Tile
        string tile = $"{this.editor.ActiveTile:D3}";
        Vector2 tileDim = this.font.MeasureString(tile);

        int tileX = baseX + (int)this.viewer.ViewSize.X + offset * 2;
        int tileY = (int)this.uiY + (int)this.viewer.ViewSize.Y - (int)tileDim.Y - offset * 2;

        // Tile Background
        batch.Draw(
            this.pixel,
            new Rectangle(tileX - offset, tileY, (int)tileDim.X + offset, (int)tileDim.Y + offset * 2),
            Colours.Metallic
        );

        // Tile Text
        batch.DrawString(
            this.font, tile, new Vector2(tileX, tileY + offset), Colours.DarkMetallic    
        );

        // Layer
        string layer = $"{this.editor.ActiveLayer:D3}";
        Vector2 layerDim = this.font.MeasureString(layer);

        int layerX = (int)this.window.GameSize.X - (int)layerDim.X - offset * 2;
        int layerY = (int)this.uiY + (int)this.viewer.ViewSize.Y - (int)layerDim.Y - offset * 2;

        this.layerRect = new Rectangle(layerX - offset, layerY, (int)layerDim.X + offset, (int)layerDim.Y + offset * 2);

        // Layer Background
        batch.Draw(
            this.pixel,
            this.layerRect,
            Colours.Pink
        );

        // Layer text
        batch.DrawString(
            this.font, layer, new Vector2(layerX, layerY + offset), Colours.PicoWhite
        );
    }
}
