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
            if (!Utilities.RefreshCache(this.window, editor)) return;
        }
    }

    private void Remove_Click(object sender, EventArgs e)
    {
        bool idConverted = int.TryParse(this.TileIDField.Text.Trim(), out int id);
        if (this.TileIDField.Text.Trim() == "")
        {
            MessageBox.Show(
                $"You cannot leave empty boxes!",
                "Input not valid!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }

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
