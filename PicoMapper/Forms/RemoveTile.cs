using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.States;
using PicoMapper.Models;
using PicoMapper.States;

namespace PicoMapper.Forms;

public partial class RemoveTile : Form
{
    private readonly Mapper window;

    private bool removed = false;

    public RemoveTile(Mapper window)
    {
        this.InitializeComponent();
        this.window = window;
    }

    private void RemoveTile_Load(object sender, EventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is IStatedState stated)
        {
            stated.State = EditorState.Dialog;
        }
    }

    private void RemoveTile_FormClosed(object sender, FormClosedEventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is Editor editor)
        {
            editor.State = EditorState.Normal;

            // Refresh cache.
            if (!this.removed) return;

            editor.TileCache.Clear();
            foreach (Tile tile in editor.Map.Tiles)
            {
                Texture2D texture;
                try
                {
                    using FileStream stream = new FileStream(tile.Path, FileMode.Open, FileAccess.Read);
                    texture = Texture2D.FromStream(this.window.GraphicsDevice, stream);
                }
                catch (Exception err)
                {
                    MessageBox.Show(
                        $"An error occoured parsing the texture: {err.Message}",
                        "Invalid Input!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );

                    return;
                }

                bool success = editor.TileCache.TryAdd(tile.ID, texture);
                if (!success)
                {
                    MessageBox.Show(
                        "Something went wrong while trying to refresh tile cache. The app will now close.",
                        "Critical Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );

                    this.window.Exit();
                }
            }
        }
    }

    private void Remove_Click(object sender, EventArgs e)
    {
        bool idConverted = int.TryParse(this.TileIDField.Text.Trim(), out int id);
        if (!idConverted)
        { 
            MessageBox.Show(
                "The value inputted is not a valid number.",
                "Invalid Input!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );
            
            return;
        }

        State? state = this.window.Context.State;
        if (state is Editor ed)
        {
            bool success = ed.TileCache.TryGetValue(id, out Texture2D? _);
            if (!success)
            {
                MessageBox.Show(
                    "The ID doesn't exist.",
                    "Invalid Input!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );

                return;
            }

            foreach (Tile tile in ed.Map.Tiles)
            {
                if (tile.ID == id)
                {
                    // Delete every instance of the tile.
                    foreach (int[,] layer in ed.Map.Layers)
                    {
                        for (int x = 0; x < layer.GetLength(0); x++)
                        {
                            for (int y = 0; y < layer.GetLength(1); y++)
                            {
                                if (layer[x, y] == tile.ID)
                                {
                                    layer[x, y] = 0;
                                }
                            }
                        }
                    }

                    ed.Map.Tiles.Remove(tile);
                    break;
                }
            }
        }

        this.removed = true;
        this.Close();
    }
}
