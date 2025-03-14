﻿using Microsoft.Xna.Framework;
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
using Keys = Microsoft.Xna.Framework.Input.Keys;
using PicoMapper.Forms;
using System.Text.Json;

namespace PicoMapper.States;

public class Editor(Mapper window, Map map, string? path = null) : State, IStatedState
{
    public readonly ComponentController NormalComponents = new ComponentController();
    private readonly ComponentController NonIgnorableComponents = new ComponentController();

    public string? FilePath = path;

    private Toggler toggler = null!;
    private MenuView menu = null!;

    public Camera2D Camera = null!;

    public readonly Map Map = map;
    public readonly Dictionary<int, Texture2D> TileCache = new Dictionary<int, Texture2D>();
    private int activeLayer = 0;
    public int ActiveTile = 0;

    public EditorState State { get; set; } = EditorState.Normal;

    private Texture2D pixel = null!;
    private SpriteFont font = null!;

    private readonly int GridThick = 3;
    private readonly int MapStart = 0;
    private Rectangle bounds;

    public Vector2 MapMouseCoords = new Vector2();
    private bool justExited = false;

    #region Drawing
    private void DrawBorders(SpriteBatch batch)
    { 
        // Top
        batch.Draw(this.pixel, new Rectangle(0, -this.GridThick, this.Map.GridX * this.Map.TileX, this.GridThick), Colours.DarkGrey);

        // Left   
        batch.Draw(this.pixel, new Rectangle(-this.GridThick, 0, this.GridThick, this.Map.GridY * this.Map.TileY), Colours.DarkGrey);

        // Bottom
        batch.Draw(this.pixel, new Rectangle(0, this.Map.GridY * this.Map.TileY, this.Map.GridX * this.Map.TileX, this.GridThick), Colours.DarkGrey);

        // Right
        batch.Draw(this.pixel, new Rectangle(this.Map.GridX * this.Map.TileX, 0, this.GridThick, this.Map.GridY * this.Map.TileY), Colours.DarkGrey);
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

    private string GetName()
    {
        if (this.FilePath is null) return "";
        string[] split = this.FilePath.Split(Path.DirectorySeparatorChar);

        return split.Last();
    }

    public void Save()
    {
        if (this.FilePath is null)
        {
            this.SaveAs();
            return;
        }

        try
        {
            string json = JsonSerializer.Serialize<Map>(this.Map);
            File.WriteAllText(this.FilePath, json);
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Save data could not be written: {e.Message}",
                "Error saving file!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }
    }

    public void SaveAs()
    {
        try
        {
            using SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Pico Mapper Files (.p8m)|*.p8m";
            file.Title = "Save map data.";

            file.ShowDialog();
            if (file.FileName.Trim() != string.Empty)
            {
                this.FilePath = file.FileName.Trim();

                string json = JsonSerializer.Serialize<Map>(this.Map);
                File.WriteAllText(this.FilePath, json);

                window.Window.Title = $"Pico Mapper ({this.GetName()})";
            }
        }
        catch (Exception e)
        { 
            MessageBox.Show(
                $"Save data could not be written: {e.Message}",
                "Error saving file!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }
    }

    // BFS Based FloodFill algo. God help me.
    // And thank you GfG
    private void FloodFill(int x, int y)
    {
        int col = this.Map.Layers[this.activeLayer].GetLength(0);
        int row = this.Map.Layers[this.activeLayer].GetLength(1);

        Queue<(int, int)> floodQueue = new Queue<(int, int)>();
        floodQueue.Enqueue((x, y));

        int old = this.Map.Layers[this.activeLayer][x, y];
        this.Map.Layers[this.activeLayer][x, y] = this.ActiveTile;

        int[] dx = [ -1, 1, 0, 0 ];
        int[] dy = [ 0, 0, -1, 1 ];

        while (floodQueue.Count > 0)
        {
            (int ox, int oy) = floodQueue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nx = ox + dx[i];
                int ny = oy + dy[i];

                if (
                    nx >= 0 && nx < col &&
                    ny >= 0 && ny < row &&
                    this.Map.Layers[this.activeLayer][nx, ny] == old
                )
                {
                    this.Map.Layers[this.activeLayer][nx, ny] = this.ActiveTile;
                    floodQueue.Enqueue((nx, ny));
                }
            }
        }
    }

    public override void LoadContent()
    {
        this.NormalComponents.Add(new TileViewer(window, this));

        this.toggler = new Toggler(window, this);
        this.NormalComponents.Add(this.toggler);

        this.menu = new MenuView(window, this);
        this.NonIgnorableComponents.Add(this.menu);

        this.Camera = new Camera2D(new Vector2(this.Map.TileX * this.Map.GridX / 2, this.Map.TileY * this.Map.GridY / 2), 2, new Vector2(window.GameSize.X / 2, window.GameSize.Y / 2));

        this.pixel = new Texture2D(window.GraphicsDevice, 1, 1);
        this.pixel.SetData([Color.White]);

        this.font = window.Content.Load<SpriteFont>("UI/Fonts/PicoNormal");

        if (this.FilePath is null)
        {
            window.Window.Title = "Pico Mapper (untitiled.p8m)";
        }
        else 
        {
            window.Window.Title = $"Pico Mapper ({this.GetName()})";
            if (!Utilities.RefreshCache(window, this)) return;
        }

        this.bounds = new Rectangle(
            this.MapStart, this.MapStart,
            this.Map.TileX * this.Map.GridX, this.Map.TileY * this.Map.GridY
        );
    }

    public override void Update(GameTime time)
    {
        switch (this.State)
        {
            case EditorState.Normal:
                this.NormalComponents.Update(time);
                this.NonIgnorableComponents.Update(time);

                // Create layer if it doesnt exist.
                if (this.Map.Layers.Count - 1 < this.activeLayer)
                {
                    int[,] layer = new int[this.Map.GridX, this.Map.GridY];
                    Array.Clear(layer, 0, layer.Length);

                    this.Map.Layers.Add(layer);
                }

                Vector2 mouse = InputHelper.GetMousePosition();
                Vector2 mouseWorld = this.Camera.ScreenToWorld(mouse);

                if (Collision.CheckRectPoint(mouseWorld, this.bounds))
                {
                    this.justExited = false;

                    this.MapMouseCoords = new Vector2(mouseWorld.X / this.Map.TileX, mouseWorld.Y / this.Map.TileY);
                    this.MapMouseCoords.Floor();

                    this.toggler.ToolTip = $"X: {this.MapMouseCoords.X} Y: {this.MapMouseCoords.Y}";

                    if (InputHelper.IsMouseDown(MouseButton.Left))
                    {
                        int[,] layer = this.Map.Layers[this.activeLayer];

                        switch (this.toggler.Active)
                        {
                            case Selected.Pencil:
                                // Skip if selected tile is reserved.
                                if (this.ActiveTile == 0)
                                    break;

                                layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] = this.ActiveTile;

                                break;

                            case Selected.Eraser:
                                // Skip if selected tile is reserved.
                                if (this.ActiveTile == 0)
                                    break;

                                layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] = 0;

                                break;

                            case Selected.Bucket:
                                if (layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] == this.ActiveTile)
                                    break;

                                this.FloodFill((int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y);

                                break;

                            default:
                                break;
                        }
                    }
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
                if (window.IsActive)
                { 
                    int delta = InputHelper.GetScrollDelta();

                    if (delta != 0)
                    {
                        Vector2 mouseBefore = this.Camera.ScreenToWorld(mouse);

                        float zoom = delta * 0.001f;
                        this.Camera.Zoom = MathHelper.Clamp(this.Camera.Zoom + zoom, 0.65f, 6f);
                        
                        Vector2 mouseAfter = this.Camera.ScreenToWorld(mouse); 

                        this.Camera.Position -= Vector2.Floor(mouseAfter - mouseBefore);
                    }

                    if (InputHelper.IsMouseDown(MouseButton.Left) && this.toggler.Active == Selected.Move)
                    {
                        this.Camera.Position -= InputHelper.GetMouseDelta() / this.Camera.Zoom;
                    }
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
        batch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: this.Camera.Transform);
            this.DrawBorders(batch);

            // Draw map
            foreach (int[,] layer in this.Map.Layers)
            {
                for (int x = 0; x < layer.GetLength(0); x++)
                {
                    for (int y = 0; y < layer.GetLength(1); y++)
                    {
                        if (layer[x, y] == 0) continue;

                        bool success = this.TileCache.TryGetValue(layer[x, y], out Texture2D? texture);
                        if (!success)
                        {
                            MessageBox.Show(
                                "Something went wrong trying to draw the map. The app will now close.",
                                "Critical Error!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error
                            );

                            Environment.Exit(1);
                        }

                        batch.Draw(texture, new Vector2(x * this.Map.TileX, y * this.Map.TileY), Color.White);
                    }
                }
            }

            // Draw cursor
            if (Collision.CheckRectPoint(this.Camera.ScreenToWorld(InputHelper.GetMousePosition()), this.bounds))
            {
                this.DrawRectangleLines(
                    this.MapMouseCoords.X * this.Map.TileX, this.MapMouseCoords.Y * this.Map.TileY,
                    this.Map.TileX, this.Map.TileY, Color.White, batch
                );
            }
        batch.End();

        // UI
        batch.Begin(samplerState: SamplerState.PointClamp);
            this.NormalComponents.Draw(batch);
            this.NonIgnorableComponents.Draw(batch);

            // batch.DrawString(this.font, $"Tiles: {this.TileCache.Count}, {this.Map.Tiles.Count}", new Vector2(5, 130), Color.White);
            // batch.DrawString(this.font, $"Zoom: {this.Camera.Zoom}", new Vector2(5, 130), Color.White);
            // batch.DrawString(this.font, $"Position: {this.Camera.Position}", new Vector2(5, 155), Color.White);
            // batch.DrawString(this.font, $"State: {this.State}", new Vector2(5, 180), Color.White);
        batch.End();
    }
}
