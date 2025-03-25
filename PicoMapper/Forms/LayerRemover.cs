using PicoMapper.States;
using SharpDX.Direct3D9;

namespace PicoMapper.Forms;

public partial class LayerRemover : Form
{
    private readonly Editor editor;

    public LayerRemover(Editor editor)
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

    private void OK_Click(object sender, EventArgs e)
    {
        int? id = this.Validate(this.IDField);
        if (id is null) return;

        if (id > this.editor.Map.Layers.Count - 1)
        {
            MessageBox.Show(
                $"Layer {id} does not exist.",
                "Input not valid!",
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

            return;
        }

        // Set all elements to 0 :3
        Array.Clear(this.editor.Map.Layers[id.Value], 0, this.editor.Map.Layers[id.Value].Length);

        this.Close();
    }
}
