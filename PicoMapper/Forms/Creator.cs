using MonoGayme.Core.States;
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

    // Idk how to get rid of this one lol
    private void textBox2_TextChanged(object sender, EventArgs e) { }

    private void Creator_Load(object sender, EventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is null) return;

        if (state is IStatedState stated)
        {
            stated.State = EditorState.Dialog;
        }
    }

    private void Creator_FormClosed(object sender, FormClosedEventArgs e)
    {
        State? state = this.window.Context.State;
        if (state is null) return;

        if (state is IStatedState stated)
        {
            stated.State = EditorState.Normal;
        }
    }
}
