using MonoGayme.Core.States;
using PicoMapper.Models;
using PicoMapper.States;

namespace PicoMapper.Forms;

public partial class Creator : Form
{
    private readonly Mapper window;

    public Creator(Mapper window)
    {
        this.InitializeComponent();

        this.window = window;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
    }

    private int? Validate(TextBox box)
    {
        if (box.Text.Trim() == "")
        {
            MessageBox.Show(
                $"You cannot leave empty boxes!",
                "Input not valid!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return null;
        }

        if (int.TryParse(box.Text.Trim(), out int number))
        {
            if (number <= 0 || number > int.MaxValue)
            { 
                MessageBox.Show(
                    "Seriously?", 
                    "Input not valid!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );

                return null;
            }

            return number;
        }

        MessageBox.Show(
            $"The value \"{box.Text}\" is not valid! Please enter a valid number.", 
            "Input not valid!", 
            MessageBoxButtons.OK, MessageBoxIcon.Error
        );

        return null;
    }

    // Idk how to get rid of this one lol
    private void textBox2_TextChanged(object sender, EventArgs e) { }

    private void Creator_Load(object sender, EventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is IStatedState stated)
        {
            stated.State = EditorState.Dialog;
        }
    }

    private void Creator_FormClosed(object sender, FormClosedEventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is IStatedState stated)
        {
            stated.State = EditorState.Normal;
        }
    }

    private void CreateMap_Click(object sender, EventArgs e)
    {
        int? mapX = this.Validate(this.MapX);
        if (mapX is null) return;

        int? mapY = this.Validate(this.MapY);
        if (mapY is null) return;

        int? tileX = this.Validate(this.TileX);
        if (tileX is null) return;

        int? tileY = this.Validate(this.TileY);
        if (tileY is null) return;

        Map map = new Map
        { 
            TileX = tileX.Value, TileY = tileY.Value,
            GridX = mapX.Value,  GridY = mapY.Value,
            Layers = [], Tiles = []
        };

        this.window.Context.SwitchState(new Editor(this.window, map));
        this.Close();
    }
}
