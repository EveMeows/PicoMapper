using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.States;
using PicoMapper.Components;
using System.Security.Policy;

namespace PicoMapper.States;

public class Editor(Mapper window) : State
{
    private readonly ComponentController NormalComponents = new ComponentController();
    private readonly ComponentController NonIgnorableComponents = new ComponentController();

    private Toggler toggler = null!;
    private MenuView menu = null!;

    public Camera2D camera = null!;

    public EditorState State { get; set; } = EditorState.Normal;

    private Texture2D pixel = null!;
    private SpriteFont font = null!;
    
    public override void LoadContent()
    {
        this.toggler = new Toggler(window);
        this.NormalComponents.Add(this.toggler);

        this.menu = new MenuView(window);
        this.NonIgnorableComponents.Add(this.menu);

        this.camera = new Camera2D(Vector2.Zero);

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoNormal");
    }

    public override void Update(GameTime time)
    {
        switch (this.State)
        {
            case EditorState.Normal:
                this.NormalComponents.Update(time);
                this.NonIgnorableComponents.Update(time);

                Vector2 mouse = InputHelper.GetMousePosition();

                // Scroll bounds
                if (mouse.Y > 20 && mouse.Y < window.GameSize.Y - 70)
                { 
                    int delta = InputHelper.GetScrollDelta();
                    if (delta != 0)
                    {
                        Vector2 mouseBefore = this.camera.ScreenToWorld(mouse);

                        float zoom = delta * 0.001f;
                        this.camera.Zoom = MathHelper.Clamp(this.camera.Zoom + zoom, 0.65f, 4.5f);
                        
                        Vector2 mouseAfter = this.camera.ScreenToWorld(mouse); 

                        this.camera.Position -= Vector2.Floor(mouseAfter - mouseBefore);
                    }

                    if (InputHelper.IsMouseDown(MouseButton.Left))
                    {
                        this.camera.Position -= Vector2.Floor(InputHelper.GetMouseDelta() / this.camera.Zoom);
                    }
                }
                break;
            case EditorState.MenuOpen:
                this.NonIgnorableComponents.Update(time);
                break;
        }
    }

    public override void Draw(GameTime time, SpriteBatch batch)
    {
        window.GraphicsDevice.Clear(Color.Black);

        // Map
        batch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: this.camera.Transform);
            // batch.Draw(this.pixel, new Rectangle(30, 30, 100, 100), Color.Yellow);
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
