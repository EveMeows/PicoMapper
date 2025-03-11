using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.States;
using PicoMapper.Components;

namespace PicoMapper.States;

public class Editor(Mapper window) : State
{
    private readonly ComponentController NormalComponents = new ComponentController();
    private readonly ComponentController NonIgnorableComponents = new ComponentController();

    private Toggler toggler = null!;
    private MenuView menu = null!;

    public EditorState State { get; set; } = EditorState.Normal;
    
    public override void LoadContent()
    {
        this.toggler = new Toggler(window);
        this.NormalComponents.Add(this.toggler);

        this.menu = new MenuView(window);
        this.NonIgnorableComponents.Add(this.menu);
    }

    public override void Update(GameTime time)
    {
        switch (this.State)
        {
            case EditorState.Normal:
                this.NormalComponents.Update(time);
                this.NonIgnorableComponents.Update(time);
                break;
            case EditorState.MenuOpen:
                this.NonIgnorableComponents.Update(time);
                break;
        }
    }

    public override void Draw(GameTime time, SpriteBatch batch)
    {
        window.GraphicsDevice.Clear(Color.Black);
        batch.Begin(samplerState: SamplerState.PointClamp);
            this.NormalComponents.Draw(batch);
            this.NonIgnorableComponents.Draw(batch);
        batch.End();
    }
}
