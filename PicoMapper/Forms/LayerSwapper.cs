using PicoMapper.States;
using SharpDX.Direct3D9;

namespace PicoMapper.Forms;

public partial class LayerSwapper : Form
{
    private readonly Editor editor;

    public LayerSwapper(Editor editor)
    {
        this.InitializeComponent();
        this.editor = editor;
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
            if (number < 0 || number > int.MaxValue)
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

    private void SwapLayers_Click(object sender, EventArgs e)
    {
        int? first = this.Validate(this.FirstLayer);
        if (first is null) return;

        int? second = this.Validate(this.SecondLayer);
        if (second is null) return;

        try
        {
            int[,] aux = this.editor.Map.Layers[first.Value];

            this.editor.Map.Layers[first.Value] = this.editor.Map.Layers[second.Value];
            this.editor.Map.Layers[second.Value] = aux;

            this.Close();
        }
        catch (Exception err)
        {
            MessageBox.Show(
                $"Layers {first} and {second} could not be swapped: {err.Message}",
                "Could not swap layers!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );
        }
    }
}
