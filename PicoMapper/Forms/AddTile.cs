using Microsoft.Xna.Framework.Graphics;
using MonoGayme.Core.States;
using PicoMapper.Models;
using PicoMapper.States;

namespace PicoMapper.Forms;

public partial class AddTile : Form
{
    private readonly Mapper window;

    private bool added = false;

    public AddTile(Mapper window)
    {
        this.InitializeComponent();

        this.window = window;
    }

    private void PathBrowser_Click(object sender, EventArgs e)
    {
        DialogResult res = this.PathChooser.ShowDialog();
        if (res == DialogResult.OK)
        {
            this.PathText.Text = this.PathChooser.FileName;
        }
    }

    private void AddTile_FormClosed(object sender, FormClosedEventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is Editor editor)
        {
            editor.State = EditorState.Normal;

            // Refresh cache.
            if (!this.added) return;

            if (!Utilities.RefreshCache(this.window, editor)) return;
        }
    }

    private void TileAdd_Click(object sender, EventArgs e)
    {
        State? state = this.window.Context.State;

        bool idConverted = int.TryParse(this.IDField.Text.Trim(), out int id);
        if (!idConverted)
        {
            MessageBox.Show(
                "The value inputted is not a valid number.",
                "Invalid Input!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }

        if (id <= 0 || id > int.MaxValue)
        {
            MessageBox.Show(
                "The value inputted is either a reserved value (0) or a negative number.",
                "Invalid Input!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }

        // Check if the ID already exists.
        // This feels like a hack??????
        if (state is Editor ed)
        {
            bool success = ed.TileCache.TryGetValue(id, out Texture2D? _);
            if (success)
            { 
                MessageBox.Show(
                    "The ID already exists.",
                    "Invalid Input!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );

                return;
            }
        }

        string path = this.PathText.Text.Trim();
        if (!Path.Exists(path))
        {
            MessageBox.Show(
                "The specified path does not exist!",
                "Invalid Input!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }

        // Arbitrary check...
        Texture2D texture;
        try
        {
            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
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

        try
        {
            if (state is Editor editor)
            {
                editor.Map.Tiles.Add(new Tile { Path = path, ID = id });
            }
        }
        catch (Exception err)
        {
            MessageBox.Show(
                $"An error occoured while adding tile: {err.Message}",
                "Invalid Input!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }

        this.added = true;
        this.Close();
    }

    private void AddTile_Load(object sender, EventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is IStatedState stated)
        {
            stated.State = EditorState.Dialog;
        }
    }
}
