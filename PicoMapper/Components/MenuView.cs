using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.UI;

namespace PicoMapper.Components;

public class MenuView : Component, IDrawableComponent, IUpdateableComponent
{
    private readonly Color BGColour = Colours.Crimson;
    private readonly int BGSize = 20;

    private readonly Texture2D pixel;
    private readonly Mapper window;

    private readonly SpriteFont font;

    private readonly UIController ui = new UIController(false);

    private readonly int OffsetY = 5;

    private void BuildUI()
    {
        TextButton MakeButton(string name, float x)
        { 
            return new TextButton(
                this.font, name, new Vector2(x, this.OffsetY), Color.White
            ) 
            {
                OnMouseEnter = (self) => { self.Colour = Color.LightGray; }, 
                OnMouseExit = (self) => { self.Colour = Color.White; }
            };
        }

        this.ui.Add(MakeButton("FILE", 2));
        this.ui.Add(MakeButton("EDIT", 40));
        this.ui.Add(MakeButton("VIEW", 78));
    }
    
    public MenuView(Mapper window)
    { 
        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.window = window;
        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoMenu");

        this.BuildUI();
    }

    public void Update(GameTime time)
    {
        this.ui.Update(InputHelper.GetMousePosition());
    }

    public void Draw(SpriteBatch batch, Camera2D? camera = null)
    {
        // Background band
        batch.Draw(
            this.pixel,
            new Rectangle(
                0, 0, 
                (int)this.window.GameSize.X, this.BGSize   
            ),
            this.BGColour
        );

        this.ui.Draw(batch, camera);
    }
}
