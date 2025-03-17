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
    public int ActiveLayer = 0;
    public int ActiveTile = 0;
    public bool OnlyActiveLayer = false;

    public EditorState State { get; set; } = EditorState.Normal;

    private Texture2D pixel = null!;
    private SpriteFont font = null!;

    private readonly int GridThick = 3;
    private readonly int MapStart = 0;
    private Rectangle bounds;

    public Vector2 MapMouseCoords = new Vector2();
    private bool justExited = false;

    public Stack<List<EditorAction>> UndoHistory = new Stack<List<EditorAction>>();
    public Stack<List<EditorAction>> RedoHistory = new Stack<List<EditorAction>>();

    public ClipboardItem? Clipboard = null;

    private Vector2? start = null;
    private Vector2? end = null;
    private Rectangle? selection = null;

    private int[,]? buffer;
    private bool bufferMoved = false;

    private bool shouldRestart = true;

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

    #region Editor
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

    public void Undo()
    {
        bool success = this.UndoHistory.TryPop(out List<EditorAction>? history);
        if (success && history is not null)
        {
            this.RedoHistory.Push(history);

            foreach (EditorAction action in history)
            {
                this.Map.Layers[action.Layer][action.X, action.Y] = action.PreviousTile;
            }
        }
    }

    public void Redo()
    {
        bool success = this.RedoHistory.TryPop(out List<EditorAction>? history);
        if (success && history is not null)
        {
            this.UndoHistory.Push(history);

            foreach (EditorAction action in history)
            {
                this.Map.Layers[action.Layer][action.X, action.Y] = action.NextTile;
            }
        }
    }

    public void Copy()
    {
        if (this.buffer is null || this.selection is null)
        {
            this.toggler.ToolTip = "No sprite selection to copy!";
            return;
        }

        this.Clipboard = new ClipboardItem { 
            Buffer = this.buffer,
            Selection = this.selection.Value
        };

        this.toggler.ToolTip = $"Copied {this.selection.Value.Width}x{this.selection.Value.Height} px";
    }

    public void Cut()
    {
        this.Copy();

        this.start = null;
        this.end = null;

        this.selection = null;
        this.bufferMoved = false;
        
        this.shouldRestart = true;

        if (this.buffer is not null && this.selection is not null)
        {
            for (int x = 0; x < this.buffer.GetLength(0); x++)
            {
                for (int y = 0; y < this.buffer.GetLength(1); y++)
                {
                    this.Map.Layers[this.ActiveLayer][x + this.selection.Value.X, y + this.selection.Value.Y] = 0;
                }
            }
        }

        this.buffer = null;
    }

    public void Paste()
    {
        if (this.Clipboard is null)
        {
            this.toggler.ToolTip = "No pixels in clipboard.";
            return;
        }

        if (this.buffer is not null && this.selection is not null)
        {
            for (int x = 0; x < this.buffer.GetLength(0); x++)
            {
                for (int y = 0; y < this.buffer.GetLength(1); y++)
                {
                    this.Map.Layers[this.ActiveLayer][x + this.selection.Value.X, y + this.selection.Value.Y] = 0;
                }
            }
        }

        this.start = new Vector2(this.Clipboard.Selection.X, this.Clipboard.Selection.Y);
        this.end = new Vector2(this.Clipboard.Selection.X + this.Clipboard.Selection.Width, this.Clipboard.Selection.Y + this.Clipboard.Selection.Height);

        this.selection = this.Clipboard.Selection;
        this.buffer = this.Clipboard.Buffer;

        this.bufferMoved = false;

        this.toggler.Active = Selected.Select;
    }
    #endregion

    private void WriteBuffer()
    {
        if (this.buffer is not null && this.selection is not null)
        {
            for (int x = 0; x < this.buffer.GetLength(0); x++)
            {
                for (int y = 0; y < this.buffer.GetLength(1); y++)
                {
                    this.Map.Layers[this.ActiveLayer][x + this.selection.Value.X, y + this.selection.Value.Y] = this.buffer[x, y];
                }
            }
        }

        this.buffer = null;
    }

    // BFS Based FloodFill algo. God help me.
    // And thank you GfG
    private void FloodFill(int x, int y)
    {
        int col = this.Map.Layers[this.ActiveLayer].GetLength(0);
        int row = this.Map.Layers[this.ActiveLayer].GetLength(1);

        List<EditorAction> history = new List<EditorAction>();

        Queue<(int, int)> floodQueue = new Queue<(int, int)>();
        floodQueue.Enqueue((x, y));

        history.Add(new EditorAction {
            X = x, Y = y, Layer = this.ActiveLayer,
            PreviousTile = this.Map.Layers[this.ActiveLayer][x, y],
            NextTile = this.ActiveTile
        });

        int old = this.Map.Layers[this.ActiveLayer][x, y];
        this.Map.Layers[this.ActiveLayer][x, y] = this.ActiveTile;

        int[] dx = [ -1, 1, 0, 0 ];
        int[] dy = [ 0, 0, -1, 1 ];

        int sx = 0; int sy = 0;
        int ex = col; int ey = row;

        if (this.selection is not null)
        {
            sx = Math.Max(0, this.selection.Value.X);
            sy = Math.Max(0, this.selection.Value.Y);
            ex = Math.Min(col, this.selection.Value.X + this.selection.Value.Width + 1);
            ey = Math.Min(row, this.selection.Value.Y + this.selection.Value.Height + 1);
        }

        while (floodQueue.Count > 0)
        {
            (int ox, int oy) = floodQueue.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int nx = ox + dx[i];
                int ny = oy + dy[i];

                if (
                    nx >= sx && nx < ex &&
                    ny >= sy && ny < ey &&
                    this.Map.Layers[this.ActiveLayer][nx, ny] == old
                )
                {
                    history.Add(new EditorAction { 
                        X = nx, Y = ny, Layer = this.ActiveLayer,
                        PreviousTile = this.Map.Layers[this.ActiveLayer][nx, ny],
                        NextTile = this.ActiveTile
                    });

                    this.Map.Layers[this.ActiveLayer][nx, ny] = this.ActiveTile;
                    floodQueue.Enqueue((nx, ny));
                }
            }
        }

        this.UndoHistory.Push(history);
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
                if (this.Map.Layers.Count - 1 < this.ActiveLayer)
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
                        int[,] layer = this.Map.Layers[this.ActiveLayer];

                        #region Active Handler
                        switch (this.toggler.Active)
                        {
                            case Selected.Pencil:
                                // Skip if selected tile is reserved.
                                if (this.ActiveTile == 0)
                                    break;

                                // Skip if are selected but try to draw outside
                                if (this.selection is not null)
                                    break;

                                if (layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] != this.ActiveTile)
                                {
                                    EditorAction paintAction = new EditorAction
                                    {
                                        X = (int)this.MapMouseCoords.X,
                                        Y = (int)this.MapMouseCoords.Y,
                                        Layer = this.ActiveLayer,
                                        PreviousTile = layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y],
                                        NextTile = this.ActiveTile
                                    };

                                    List<EditorAction> paintQueue = [paintAction];
                                    this.UndoHistory.Push(paintQueue);
                                }

                                layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] = this.ActiveTile;
                                break;

                            case Selected.Eraser:
                                // Skip if selected tile is reserved.
                                if (this.ActiveTile == 0)
                                    break;

                                // Skip if are selected but try to draw outside
                                if (this.selection is not null)
                                    break;

                                if (layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] != 0)
                                {
                                    EditorAction paintAction = new EditorAction
                                    {
                                        X = (int)this.MapMouseCoords.X,
                                        Y = (int)this.MapMouseCoords.Y,
                                        Layer = this.ActiveLayer,
                                        PreviousTile = layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y],
                                        NextTile = 0
                                    };

                                    List<EditorAction> paintQueue = [paintAction];
                                    this.UndoHistory.Push(paintQueue);
                                }

                                layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] = 0;
                                break;

                            case Selected.Bucket:
                                if (layer[(int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y] == this.ActiveTile)
                                    break;

                                // Skip if are selected but try to draw outside
                                if (this.selection is not null)
                                    break;

                                this.FloodFill((int)this.MapMouseCoords.X, (int)this.MapMouseCoords.Y);
                                break;

                            case Selected.Select:
                                this.WriteBuffer();
                                
                                if (this.start is null || this.shouldRestart)
                                {
                                    this.start = this.MapMouseCoords;
                                    this.shouldRestart = false;
                                    break;
                                }

                                if (this.end != this.MapMouseCoords)
                                {
                                    this.end = this.MapMouseCoords;

                                    // Calculate Rectangle
                                    int rectX = (int)Math.Min(this.start.Value.X, this.end.Value.X);
                                    int rectY = (int)Math.Min(this.start.Value.Y, this.end.Value.Y);
                                    int rectWidth = (int)Math.Abs(this.end.Value.X - this.start.Value.X);
                                    int rectHeight = (int)Math.Abs(this.end.Value.Y - this.start.Value.Y);

                                    this.selection = new Rectangle(rectX, rectY, rectWidth, rectHeight);
                                }
                                break;

                            default:
                                break;
                        }
                        #endregion
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
                break;

            case EditorState.MenuOpen:
                this.NonIgnorableComponents.Update(time);
                break;

            default:
                break;
        }

        #region Selection Movement
        if (this.selection is not null)
        {
            // End selection
            if (InputHelper.IsKeyPressed(Keys.Enter))
            {
                this.WriteBuffer();

                this.start = null;
                this.end = null;

                this.selection = null;
                this.bufferMoved = false;
            }

            if (InputHelper.IsKeyPressed(Keys.Right) && this.shouldRestart)
            {
                if (this.selection is not null)
                {
                    Rectangle newRight = this.selection.Value;
                    newRight.X++;

                    if (newRight.X + newRight.Width + 1 > this.Map.Layers[this.ActiveLayer].GetLength(0))
                    {
                        newRight.Width--;
                        if (newRight.Width < 0)
                        {
                            newRight.X--;
                            newRight.Width = 0;
                        }

                        if (this.buffer is not null)
                        {
                            int[,] old = this.buffer;
                            this.buffer = new int[newRight.Width + 1, newRight.Height + 1];
                            for (int y = 0; y < newRight.Height + 1; y++)
                            {
                                for (int x = 0; x < newRight.Width + 1; x++)
                                {
                                    this.buffer[x, y] = old[x, y];
                                }
                            }
                        }
                    }

                    this.selection = newRight;
                }
            }

            if (InputHelper.IsKeyPressed(Keys.Left) && this.shouldRestart)
            {
                if (this.selection is not null)
                {
                    Rectangle newLeft = this.selection.Value;
                    newLeft.X--;

                    if (newLeft.X < 0)
                    {
                        newLeft.X++;

                        newLeft.Width--;
                        if (newLeft.Width < 0)
                        {
                            newLeft.Width = 0;
                        }

                        if (this.buffer is not null)
                        {
                            int[,] old = this.buffer;
                            this.buffer = new int[newLeft.Width + 1, newLeft.Height + 1];
                            for (int y = 0; y < newLeft.Height + 1; y++)
                            {
                                for (int x = newLeft.Width; x >= 0; x--)
                                {
                                    this.buffer[x, y] = old[x, y];
                                }
                            }
                        }
                    }

                    this.selection = newLeft;
                }
            }

            if (InputHelper.IsKeyPressed(Keys.Up) && this.shouldRestart)
            {
                if (this.selection is not null)
                {
                    Rectangle newUp = this.selection.Value;
                    newUp.Y--;

                    if (newUp.Y < 0)
                    {
                        newUp.Y++;

                        newUp.Height--;
                        if (newUp.Height < 0)
                        {
                            newUp.Height = 0;
                        }

                        if (this.buffer is not null)
                        {
                            int[,] old = this.buffer;
                            this.buffer = new int[newUp.Width + 1, newUp.Height + 1];
                            for (int y = 0; y < newUp.Height + 1; y++)
                            {
                                for (int x = 0; x < newUp.Width + 1; x++)
                                {
                                    this.buffer[x, y] = old[x, y];
                                }
                            }
                        }
                    }

                    this.selection = newUp;
                }
            }

            if (InputHelper.IsKeyPressed(Keys.Down) && this.shouldRestart)
            {
                if (this.selection is not null)
                {
                    Rectangle newDown = this.selection.Value;
                    newDown.Y++;

                    if (newDown.Y + newDown.Height + 1 > this.Map.Layers[this.ActiveLayer].GetLength(1))
                    {
                        newDown.Height--;
                        if (newDown.Height < 0)
                        {
                            newDown.Y--;
                            newDown.Height = 0;
                        }

                        if (this.buffer is not null)
                        {
                            int[,] old = this.buffer;
                            this.buffer = new int[newDown.Width + 1, newDown.Height + 1];
                            for (int y = newDown.Height; y >= 0; y--)
                            {
                                for (int x = 0; x < newDown.Width + 1; x++)
                                {
                                    this.buffer[x, y] = old[x, y];
                                }
                            }
                        }
                    }

                    this.selection = newDown;
                }
            }
        }
        #endregion

        if (this.toggler.Active == Selected.Select)
        {
            if (InputHelper.IsMouseUp(MouseButton.Left))
            {
                if (this.buffer is null && this.selection is not null)
                {
                    this.buffer = new int[this.selection.Value.Width + 1, this.selection.Value.Height + 1];
                    for (int y = 0; y < this.selection.Value.Height + 1; y++)
                    {
                        for (int x = 0; x < this.selection.Value.Width + 1; x++)
                        {
                            this.buffer[x, y] = this.Map.Layers[this.ActiveLayer][this.selection.Value.X + x, this.selection.Value.Y + y];
                            this.Map.Layers[this.ActiveLayer][x + this.selection.Value.X, y + this.selection.Value.Y] = 0;
                        }
                    }
                }

                this.shouldRestart = true;
            }

            // Delete Selection
            if (InputHelper.IsKeyPressed(Keys.Delete))
            {
                if (!this.bufferMoved && this.buffer is not null && this.selection is not null)
                {
                    for (int y = 0; y < this.selection.Value.Height + 1; y++)
                    {
                        for (int x = 0; x < this.selection.Value.Width + 1; x++)
                        {
                            this.Map.Layers[this.ActiveLayer][x + this.selection.Value.X, y + this.selection.Value.Y] = 0;
                        }
                    }
                }

                this.buffer = null;
            }
        }
    }

    public override void Draw(GameTime time, SpriteBatch batch)
    {
        window.GraphicsDevice.Clear(Color.Black);

        // Map
        batch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: this.Camera.Transform);
        { 
            this.DrawBorders(batch);

            // Draw map
            foreach (int[,] layer in this.Map.Layers)
            {
                for (int x = 0; x < layer.GetLength(0); x++)
                {
                    for (int y = 0; y < layer.GetLength(1); y++)
                    {
                        if (this.OnlyActiveLayer && this.Map.Layers.IndexOf(layer) != this.ActiveLayer) continue;

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

            // Draw selection buffer on top of map
            if (this.buffer is not null && this.selection is not null)
            {
                for (int x = 0; x < this.buffer.GetLength(0); x++)
                {
                    for (int y = 0; y < this.buffer.GetLength(1); y++)
                    {
                        if (this.buffer[x, y] == 0) continue;

                        bool success = this.TileCache.TryGetValue(this.buffer[x, y], out Texture2D? texture);
                        if (!success)
                        {
                            MessageBox.Show(
                                "Something went wrong trying to draw the map. The app will now close.",
                                "Critical Error!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error
                            );

                            Environment.Exit(1);
                        }

                        int wx = (this.selection.Value.X + x) * this.Map.TileX;
                        int wy = (this.selection.Value.Y + y) * this.Map.TileY;

                        batch.Draw(texture, new Vector2(wx, wy), Color.White);
                    }
                }
            }

            if (this.start is not null && this.end is not null)
            {
                if (this.selection is not null)
                {
                    this.DrawRectangleLines(
                        this.selection.Value.X * this.Map.TileX,
                        this.selection.Value.Y * this.Map.TileY,
                        (this.selection.Value.Width + 1) * this.Map.TileX,
                        (this.selection.Value.Height + 1) * this.Map.TileY,
                        Colours.Grey, batch
                    );
                }

                // batch.Draw(this.pixel, new Rectangle((int)this.start.Value.X * this.Map.TileX, (int)this.start.Value.Y * this.Map.TileY, 8, 8), Color.Orange);
                // batch.Draw(this.pixel, new Rectangle((int)this.end.Value.X * this.Map.TileX, (int)this.end.Value.Y * this.Map.TileY, 8, 8), Color.Orange);
            }

            // Draw cursor
            // We skip if we are in select mode
            if (!InputHelper.IsMouseDown(MouseButton.Left) || this.toggler.Active != Selected.Select)
            {
                if (Collision.CheckRectPoint(this.Camera.ScreenToWorld(InputHelper.GetMousePosition()), this.bounds) && window.IsActive)
                {
                    this.DrawRectangleLines(
                        this.MapMouseCoords.X * this.Map.TileX, this.MapMouseCoords.Y * this.Map.TileY,
                        this.Map.TileX, this.Map.TileY, Color.White, batch
                    );
                }
            }
        }
        batch.End();

        // UI
        batch.Begin(samplerState: SamplerState.PointClamp);
            this.NormalComponents.Draw(batch);
            this.NonIgnorableComponents.Draw(batch);

            // batch.DrawString(this.font, $"Undo: {this.UndoHistory.Count}", new Vector2(5, 130), Color.White);
            // batch.DrawString(this.font, $"Redo: {this.RedoHistory.Count}", new Vector2(5, 155), Color.White);

            // batch.DrawString(this.font, $"Zoom: {this.Camera.Zoom}", new Vector2(5, 130), Color.White);
            // batch.DrawString(this.font, $"Position: {this.Camera.Position}", new Vector2(5, 155), Color.White);
            // batch.DrawString(this.font, $"State: {this.State}", new Vector2(5, 180), Color.White);
        batch.End();
    }
}
