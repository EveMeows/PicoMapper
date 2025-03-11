using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Abstractions;
using MonoGayme.Core.Components;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.States;
using MonoGayme.Core.UI;
using MonoGayme.Core.Utilities;
using PicoMapper.States;
using PicoMapper.UI;

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

    private Menu? active;

    private Dictionary<string, Menu> Menus = new Dictionary<string, Menu>();

    private void BuildUI()
    {
        TextButton MakeButton(string name, float x)
        { 
            return new TextButton(
                this.font, name, new Vector2(x, this.OffsetY), Color.White
            ) 
            {
                OnMouseEnter = (self) => { self.Colour = Color.LightGray; }, 
                OnMouseExit = (self) => { self.Colour = Color.White; },
                OnClick = (self) => {
                    State? state = this.window.Context.State;
                    if (state is null) return;

                    if (state is Editor editor)
                    { 
                        if (this.active == this.Menus[name])
                        {
                            this.active = null;
                            editor.State = EditorState.Normal;
                            return;
                        }

                        this.active = this.Menus[name];
                    }
                }
            };
        }

        TextButton MakeMenuButton(string name, Vector2 position)
        {
            return new TextButton(
                this.font,
                name, position, Color.White
            )
            {
                OnMouseEnter = (self) => { self.Colour = Color.LightGray; }, 
                OnMouseExit = (self) => { self.Colour = Color.White; },
            };
        }

        const float offset = 10;

        Menu file = new Menu(new Rectangle(0, this.BGSize, 183, 105), this.window);
        file.Buttons.Add(MakeMenuButton("NEW               CTRL + N", new Vector2(5, this.BGSize + offset)));
        file.Buttons.Add(MakeMenuButton("OPEN...        CTRL + O", new Vector2(5, this.BGSize + offset * 3)));
        file.Buttons.Add(MakeMenuButton("SAVE            CTRL + S", new Vector2(5, this.BGSize + offset * 5)));
        file.Buttons.Add(MakeMenuButton("SAVE AS...  CTRL + SHIFT + S", new Vector2(5, this.BGSize + offset * 7)));
        file.Buttons.Add(MakeMenuButton("EXIT", new Vector2(5, this.BGSize + offset * 9)));
        this.Menus.Add("FILE", file);

        Menu edit = new Menu(new Rectangle(40, this.BGSize, 110, 105), this.window);
        edit.Buttons.Add(MakeMenuButton("UNDO     CTRL + Z", new Vector2(45, this.BGSize + offset)));
        edit.Buttons.Add(MakeMenuButton("REDO     CTRL + Y", new Vector2(45, this.BGSize + offset * 3)));
        edit.Buttons.Add(MakeMenuButton("CUT        CTRL + X", new Vector2(45, this.BGSize + offset * 5)));
        edit.Buttons.Add(MakeMenuButton("COPY     CTRL + C", new Vector2(45, this.BGSize + offset * 7)));
        edit.Buttons.Add(MakeMenuButton("PASTE  CTRL + V", new Vector2(45, this.BGSize + offset * 9)));
        this.Menus.Add("EDIT", edit);

        Menu view = new Menu(new Rectangle(78, this.BGSize, 89, 45), this.window);
        view.Buttons.Add(MakeMenuButton("ZOOM IN       +", new Vector2(83, this.BGSize + offset)));
        view.Buttons.Add(MakeMenuButton("ZOOM OUT    -", new Vector2(83, this.BGSize + offset * 3)));
        this.Menus.Add("VIEW", view);

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

        if (this.active is not null)
        { 
            this.active.Update(time);

            if (
                !Collision.CheckRectPoint(InputHelper.GetMousePosition(), this.active.Size) &&
                !Collision.CheckRectPoint(InputHelper.GetMousePosition(), new Rectangle(0, 0, (int)this.window.GameSize.X, this.BGSize))
            )
            {
                if (InputHelper.IsMousePressed(MouseButton.Left))
                {
                    this.active = null;
                    
                    State? state = this.window.Context.State;
                    if (state is null) return;

                    if (state is Editor editor)
                        editor.State = EditorState.Normal;
                }
            }
        }
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
        this.active?.Draw(batch, camera);
    }
}
