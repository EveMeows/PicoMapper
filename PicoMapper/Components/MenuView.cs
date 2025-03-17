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

using Color = Microsoft.Xna.Framework.Color;
using Button = MonoGayme.Core.UI.Button;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using PicoMapper.Forms;

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

    private Dictionary<string, Menu> menus = new Dictionary<string, Menu>();

    private TileViewer? viewer = null;

    private readonly Editor editor;

    private readonly float ButtonDelay = 150;

    private bool isUndoing = false;
    private float undoTime = 0;

    private bool isRedoing = false;
    private float redoTime = 0;

    #region Events
    private void Create(Button? self = null)
    { 
        using Creator creator = new Creator(this.window);
        creator.ShowDialog();
    }

    private void NewTile(Button? self = null)
    {
        using AddTile newTile = new AddTile(this.window);
        newTile.ShowDialog();
    }

    private void RemoveTile(Button? self = null)
    { 
        using RemoveTile removeTile = new RemoveTile(this.window);
        removeTile.ShowDialog();
    }

    private void ToggleHandle(Button? self = null)
    {
        if (this.viewer is null) return;
        this.viewer.HandleVisible = !this.viewer.HandleVisible;
    }

    private void ToggleTiles(Button? self = null)
    {
        if (this.viewer is null) return;
        this.viewer.HandleOpen = !this.viewer.HandleOpen;
    }

    private void Centre(Button? self = null)
    {
        this.editor.Camera.Origin = this.window.GameSize / 2;
        this.editor.Camera.Position = new Vector2(this.editor.Map.TileX * this.editor.Map.GridX / 2, this.editor.Map.TileY * this.editor.Map.GridY / 2);
    }

    private void ZoomOut(Button? self = null)
    {
        this.editor.Camera.Zoom = MathHelper.Clamp(this.editor.Camera.Zoom - 0.3f, 0.65f, 6);
    }

    private void ZoomIn(Button? self = null)
    {
        this.editor.Camera.Zoom = MathHelper.Clamp(this.editor.Camera.Zoom + 0.3f, 0.65f, 6);
    }

    private void ActiveLayer(Button? self = null)
    { 
        this.editor.OnlyActiveLayer = !this.editor.OnlyActiveLayer;
    }
    #endregion

    private void Reset()
    { 
        this.active = null;
        this.editor.State = EditorState.Normal;
    }

    private void BuildUI()
    {
        TextButton MakeButton(string name, float x)
        {  
            TextButton btn = new TextButton(this.font, name, new Vector2(x, this.OffsetY), Color.White) 
            {
                OnMouseEnter = (self) => { self.Colour = Color.LightGray; }, 
                OnMouseExit  = (self) => { self.Colour = Color.White; },

                OnClick = (self) => {
                    if (this.active == this.menus[name])
                    {
                        this.active = null;
                        this.editor.State = EditorState.Normal;
                        return;
                    }

                    this.active = this.menus[name];
                    this.editor.State = EditorState.MenuOpen;
                }
            };

            return btn;
        }

        TextButton MakeMenuButton(string name, Vector2 position, Action<Button>? click = null)
        {
            TextButton btn = new TextButton(this.font, name, position, Color.White)
            {
                OnMouseEnter = (self) => { self.Colour = Color.LightGray; },
                OnMouseExit  = (self) => { self.Colour = Color.White; },
                OnClick = click
            };

            return btn;
        }

        const float offset = 10;

        Menu file = new Menu(new Rectangle(0, this.BGSize, 183, 105), this.window);
        file.Buttons.Add(MakeMenuButton("NEW               CTRL + N", new Vector2(5, this.BGSize + offset), this.Create));
        file.Buttons.Add(MakeMenuButton("OPEN...        CTRL + O", new Vector2(5, this.BGSize + offset * 3), (self) => { Utilities.Open(this.window); }));
        file.Buttons.Add(MakeMenuButton("SAVE            CTRL + S", new Vector2(5, this.BGSize + offset * 5), (self) => { this.editor.Save(); }));
        file.Buttons.Add(MakeMenuButton("SAVE AS...  CTRL + SHIFT + S", new Vector2(5, this.BGSize + offset * 7), (self) => { this.editor.SaveAs(); }));
        file.Buttons.Add(MakeMenuButton("EXIT", new Vector2(5, this.BGSize + offset * 9), (self) => { this.window.Exit(); }));
        this.menus.Add("FILE", file);

        Menu edit = new Menu(new Rectangle(40, this.BGSize, 110, 105), this.window);
        edit.Buttons.Add(MakeMenuButton("UNDO     CTRL + Z", new Vector2(45, this.BGSize + offset), (self) => { this.editor.Undo(); }));
        edit.Buttons.Add(MakeMenuButton("REDO     CTRL + Y", new Vector2(45, this.BGSize + offset * 3), (self) => { this.editor.Redo(); }));
        edit.Buttons.Add(MakeMenuButton("CUT        CTRL + X", new Vector2(45, this.BGSize + offset * 5), (self) => { this.editor.Cut(); }));
        edit.Buttons.Add(MakeMenuButton("COPY     CTRL + C", new Vector2(45, this.BGSize + offset * 7), (self) => { this.editor.Copy(); }));
        edit.Buttons.Add(MakeMenuButton("PASTE  CTRL + V", new Vector2(45, this.BGSize + offset * 9), (self) => { this.editor.Paste(); }));
        this.menus.Add("EDIT", edit);

        Menu view = new Menu(new Rectangle(78, this.BGSize, 250, 165), this.window);
        view.Buttons.Add(MakeMenuButton("ZOOM IN", new Vector2(83, this.BGSize + offset), this.ZoomIn));
        view.Buttons.Add(MakeMenuButton("ZOOM OUT", new Vector2(83, this.BGSize + offset * 3), this.ZoomOut));
        view.Buttons.Add(MakeMenuButton("CENTRE CAMERA           CTRL + SHIFT + C", new Vector2(83, this.BGSize + offset * 5), this.Centre));
        view.Buttons.Add(MakeMenuButton("TOGGLE HANDLE           CTRL + H", new Vector2(83, this.BGSize + offset * 9), this.ToggleHandle));
        view.Buttons.Add(MakeMenuButton("TOGGLE TILE VIEW     CTRL + T", new Vector2(83, this.BGSize + offset * 11), this.ToggleTiles));
        view.Buttons.Add(MakeMenuButton("SHOW ACTIVE LAYER     CTRL + L", new Vector2(83, this.BGSize + offset * 15), this.ActiveLayer));
        this.menus.Add("VIEW", view);

        Menu tiles = new Menu(new Rectangle(115, this.BGSize, 158, 45), this.window);
        tiles.Buttons.Add(MakeMenuButton("ADD TILE            CTRL + A", new Vector2(120, this.BGSize + offset), this.NewTile));
        tiles.Buttons.Add(MakeMenuButton("REMOVE TILE    CTRL + R", new Vector2(120, this.BGSize + offset * 3), this.RemoveTile));
        this.menus.Add("TILES", tiles);

        this.ui.Add(MakeButton("FILE", 2));
        this.ui.Add(MakeButton("EDIT", 40));
        this.ui.Add(MakeButton("VIEW", 78));
        this.ui.Add(MakeButton("TILES", 115));
    }
    
    public MenuView(Mapper window, Editor editor)
    { 
        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.window = window;
        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoMenu");

        this.editor = editor;
        this.viewer = this.editor.NormalComponents.Get<TileViewer>();

        this.BuildUI();
    }

    public void Update(GameTime time)
    {
        float delta = (float)time.ElapsedGameTime.TotalMilliseconds;

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
                    this.Reset();
                }
            }

            if (InputHelper.IsKeyPressed(Keys.Escape))
            {
                this.Reset();
            }
        }

        if (InputHelper.IsKeyDown(Keys.LeftControl))
        {
            if (InputHelper.IsKeyDown(Keys.LeftShift))
            {
                if (InputHelper.IsKeyPressed(Keys.C))
                {
                    this.Centre();
                }

                if (InputHelper.IsKeyPressed(Keys.S))
                {
                    this.editor.SaveAs();
                }
            }

            if (InputHelper.IsKeyPressed(Keys.X))
            {
                this.editor.Cut();
            }

            if (InputHelper.IsKeyPressed(Keys.C))
            {
                this.editor.Copy();
            }

            if (InputHelper.IsKeyPressed(Keys.V))
            {
                this.editor.Paste();
            }

            if (InputHelper.IsKeyDown(Keys.Z))
            {
                if (!this.isUndoing)
                {
                    this.editor.Undo();
                }

                this.isUndoing = true;
            }

            if (InputHelper.IsKeyPressed(Keys.Y))
            {
                if (!this.isRedoing)
                {
                    this.editor.Redo();
                }

                this.isRedoing = true;
            }

            if (InputHelper.IsKeyPressed(Keys.S))
            {
                this.editor.Save();
            }

            if (InputHelper.IsKeyPressed(Keys.O))
            {
                Utilities.Open(this.window);
            }

            if (InputHelper.IsKeyPressed(Keys.N))
            {
                this.Create();
            }


            if (InputHelper.IsKeyPressed(Keys.A))
            {
                this.NewTile();
            }

            if (InputHelper.IsKeyPressed(Keys.R))
            {
                this.RemoveTile();
            }

            if (InputHelper.IsKeyPressed(Keys.H))
            {
                this.ToggleHandle();
            }

            if (InputHelper.IsKeyPressed(Keys.T))
            {
                this.ToggleTiles();
            }

            if (InputHelper.IsKeyPressed(Keys.L))
            {
                this.ActiveLayer();
            }
        }

        if (this.isUndoing)
        { 
            this.undoTime += delta;
            if (this.undoTime >= this.ButtonDelay)
            {
                this.undoTime = 0;
                this.editor.Undo();
            }
        }

        if (this.isRedoing)
        {
            this.redoTime += delta;
            if (this.redoTime >= this.ButtonDelay)
            {
                this.redoTime = 0;
                this.editor.Redo();
            }
        }

        if (InputHelper.IsKeyUp(Keys.Z) && this.isUndoing)
        {
            this.undoTime = 0;
            this.isUndoing = false;
        }

        if (InputHelper.IsKeyUp(Keys.Y) && this.isRedoing)
        {
            this.redoTime = 0;
            this.isRedoing = false;
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
