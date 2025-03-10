using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.States;
using PicoMapper.Components;

namespace PicoMapper.States;

public class Editor(Mapper window) : State
{
    private readonly ComponentController components = new ComponentController();

    private Toggler toggler = null!;
    private MenuView menu = null!;
    
    public override void LoadContent()
    {
        this.toggler = new Toggler(window);
        this.components.Add(this.toggler);

        this.menu = new MenuView(window);
        this.components.Add(this.menu);
    }

    public override void Update(GameTime time)
    {
        this.components.Update(time);
    }

    public override void Draw(GameTime time, SpriteBatch batch)
    {
        window.GraphicsDevice.Clear(Color.Black);
        batch.Begin(samplerState: SamplerState.PointClamp);
            this.components.Draw(batch);
        batch.End();
    }
}
