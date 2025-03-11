using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.UI;

namespace PicoMapper.UI;

public class Menu : Component, IUpdateableComponent, IDrawableComponent
{
    public readonly UIController Buttons = new UIController(false);
    public readonly Rectangle Size;

    private readonly Texture2D pixel;

    public Menu(Rectangle size, Mapper window) 
    {
        this.Size = size;

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Colours.Crimson]);
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
            Color.White
        );

        this.Buttons.Draw(batch, camera);
    }
}
