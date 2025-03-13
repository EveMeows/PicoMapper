using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.States;
using MonoGayme.Core.Utilities;
using PicoMapper.Components;
using PicoMapper.Models;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace PicoMapper.States;

public class Editor(Mapper window, Map map, string? path = null) : State, IStatedState
{
    private readonly ComponentController NormalComponents = new ComponentController();
    private readonly ComponentController NonIgnorableComponents = new ComponentController();

    private Toggler toggler = null!;
    private MenuView menu = null!;

    public Camera2D camera = null!;

    public EditorState State { get; set; } = EditorState.Normal;

    private Texture2D pixel = null!;
    private SpriteFont font = null!;

    private readonly int GridThick = 3;
    private readonly int MapStart = 0;
    private Rectangle bounds;

    private Vector2 mapMouse = new Vector2();
    private bool justExited = false;

    #region Drawing
    private void DrawBorders(SpriteBatch batch)
    { 
        // Top
        batch.Draw(this.pixel, new Rectangle(0, -this.GridThick, map.GridX * map.TileX, this.GridThick), Colours.DarkGrey);

        // Left   
        batch.Draw(this.pixel, new Rectangle(-this.GridThick, 0, this.GridThick, map.GridY * map.TileY), Colours.DarkGrey);

        // Bottom
        batch.Draw(this.pixel, new Rectangle(0, map.GridY * map.TileY, map.GridX * map.TileX, this.GridThick), Colours.DarkGrey);

        // Right
        batch.Draw(this.pixel, new Rectangle(map.GridX * map.TileX, 0, this.GridThick, map.GridY * map.TileY), Colours.DarkGrey);
    }

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
    #endregion

    public override void LoadContent()
    {
        this.toggler = new Toggler(window);
        this.NormalComponents.Add(this.toggler);

        this.menu = new MenuView(window);
        this.NonIgnorableComponents.Add(this.menu);

        this.camera = new Camera2D(new Vector2(map.TileX * map.GridX / 2, map.TileY * map.GridY / 2), 2, new Vector2(window.GameSize.X / 2, window.GameSize.Y / 2));

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoNormal");

        if (path is null)
        {
            window.Window.Title = "PicoMapper (untitiled.pm)";
        }

        this.bounds = new Rectangle(
            this.MapStart, this.MapStart,
            map.TileX * map.GridX, map.TileY * map.GridY
        );
    }

    public override void Update(GameTime time)
    {
        switch (this.State)
        {
            case EditorState.Normal:
                this.NormalComponents.Update(time);
                this.NonIgnorableComponents.Update(time);

                Vector2 mouse = InputHelper.GetMousePosition();
                Vector2 mouseWorld = this.camera.ScreenToWorld(mouse);

                // Show map coords.
                if (Collision.CheckRectPoint(mouseWorld, this.bounds))
                {
                    this.justExited = false;

                    this.mapMouse = new Vector2(mouseWorld.X / map.TileX, mouseWorld.Y / map.TileY);
                    this.mapMouse.Floor();

                    this.toggler.ToolTip = $"X: {this.mapMouse.X} Y: {this.mapMouse.Y}";
                }
                else
                {
                    if (!this.justExited)
                    {
                        this.justExited = true;
                        this.toggler.ToolTip = "";
                    }
                }

                // Camera controls
                int delta = InputHelper.GetScrollDelta();

                if (delta != 0)
                {
                    Vector2 mouseBefore = this.camera.ScreenToWorld(mouse);

                    float zoom = delta * 0.001f;
                    this.camera.Zoom = MathHelper.Clamp(this.camera.Zoom + zoom, 0.65f, 6f);
                    
                    Vector2 mouseAfter = this.camera.ScreenToWorld(mouse); 

                    this.camera.Position -= Vector2.Floor(mouseAfter - mouseBefore);
                }

                if (InputHelper.IsMouseDown(MouseButton.Left) && this.toggler.Active == Selected.Move)
                {
                    this.camera.Position -= InputHelper.GetMouseDelta() / this.camera.Zoom;
                }

                break;

            case EditorState.MenuOpen:
                this.NonIgnorableComponents.Update(time);
                break;

            default:
                break;
        }
    }

    public override void Draw(GameTime time, SpriteBatch batch)
    {
        window.GraphicsDevice.Clear(Color.Black);

        // Map
        batch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: this.camera.Transform);
            this.DrawBorders(batch);

            // Draw cursor
            if (Collision.CheckRectPoint(this.camera.ScreenToWorld(InputHelper.GetMousePosition()), this.bounds))
            {
                this.DrawRectangleLines(
                    this.mapMouse.X * map.TileX, this.mapMouse.Y * map.TileY,
                    map.TileX, map.TileY, Color.White, batch
                );
            }
        batch.End();

        // UI
        batch.Begin(samplerState: SamplerState.PointClamp);
            this.NormalComponents.Draw(batch);
            this.NonIgnorableComponents.Draw(batch);

            // batch.DrawString(this.font, $"Zoom: {this.camera.Zoom}", new Vector2(5, 130), Color.White);
            // batch.DrawString(this.font, $"Position: {this.camera.Position}", new Vector2(5, 155), Color.White);
            // batch.DrawString(this.font, $"State: {this.State}", new Vector2(5, 180), Color.White);
        batch.End();
    }
}
