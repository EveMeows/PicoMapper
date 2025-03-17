using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.Controllers;
using MonoGayme.Core.Input;
using MonoGayme.Core.States;
using MonoGayme.Core.UI;
using PicoMapper.Forms;
using System.Diagnostics;
using System.Security.Policy;
using Button = MonoGayme.Core.UI.Button;
using Color = Microsoft.Xna.Framework.Color;

namespace PicoMapper.States;

public class MainMenu(Mapper window) : State
{
    private readonly UIController UI = new UIController(false);

    private string version = string.Empty;
    private Vector2 versionDimension;

    private SpriteFont fontSmall = null!;
    private SpriteFont fontNormal = null!;

    private void BuildUI()
    {
        TextButton CreateButton(string text, float y, Action<Button> action)
        {
            return new TextButton(this.fontNormal, text, new Vector2(20, y), Color.White)
            {
                OnClick = action,

                OnMouseEnter = (self) => {
                    self.Colour = Color.Yellow;
                },

                OnMouseExit = (self) => {
                    self.Colour = Color.White;
                }
            };
        }

        this.UI.Add(CreateButton("New Map", 30, (self) => {
            using Creator creator = new Creator(window);
            creator.ShowDialog();
        }));

        this.UI.Add(CreateButton("Open Map...", 60, (self) => {
            Utilities.Open(window);
        }));

        this.UI.Add(CreateButton("Found a bug?", 90, (self) => {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/EveMeows/PicoMapper/issues/new",
                UseShellExecute = true
            });
        }));
    }

    public override void LoadContent()
    {
        this.fontSmall = window.Content.Load<SpriteFont>("UI/Fonts/PicoMenu");
        this.fontNormal = window.Content.Load<SpriteFont>("UI/Fonts/PicoNormal");

        this.BuildUI();

        this.version = $"v{window.Version} by Evelyn Serra";
        this.versionDimension = this.fontSmall.MeasureString(this.version);
    }

    public override void Update(GameTime time)
    {
        this.UI.Update(InputHelper.GetMousePosition());
    }

    public override void Draw(GameTime time, SpriteBatch batch)
    {
        window.GraphicsDevice.Clear(Colours.Crimson);

        batch.Begin();
        {
            this.UI.Draw(batch);

            Vector2 versionPosition = new Vector2((window.GameSize.X - this.versionDimension.X) / 2, window.GameSize.Y - this.versionDimension.Y - 10);
            versionPosition.Floor();
            batch.DrawString(
                this.fontSmall, this.version,
                versionPosition,
                Colours.BrightRed
            );

            batch.DrawString(
                this.fontNormal, "Pico Mapper",
                new Vector2(window.GameSize.X - 250, 40), Color.White
            );

            batch.DrawString(
                this.fontSmall, "Tiny TileMap Editor inspired by the Pico8.",
                new Vector2(window.GameSize.X - 330, 80), Color.White
            );

            batch.DrawString(
                this.fontSmall, "Made with love <3",
                new Vector2(window.GameSize.X - 230, 100), Color.White
            );
        }
        batch.End();
    }
}
