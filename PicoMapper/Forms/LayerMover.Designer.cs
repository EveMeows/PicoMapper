namespace PicoMapper.Forms;

partial class LayerMover
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.label1 = new Label();
        this.label2 = new Label();
        this.Cancel = new Button();
        this.OK = new Button();
        this.IDField = new TextBox();
        this.label3 = new Label();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 31);
        this.label1.Name = "label1";
        this.label1.Size = new Size(44, 20);
        this.label1.TabIndex = 0;
        this.label1.Text = "Layer";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new Point(12, 78);
        this.label2.Name = "label2";
        this.label2.Size = new Size(200, 20);
        this.label2.TabIndex = 1;
        this.label2.Text = "NOTE: Moves the active layer";
        // 
        // Cancel
        // 
        this.Cancel.Location = new Point(112, 139);
        this.Cancel.Name = "Cancel";
        this.Cancel.Size = new Size(94, 29);
        this.Cancel.TabIndex = 2;
        this.Cancel.Text = "Cancel";
        this.Cancel.UseVisualStyleBackColor = true;
        // 
        // OK
        // 
        this.OK.Location = new Point(12, 139);
        this.OK.Name = "OK";
        this.OK.Size = new Size(94, 29);
        this.OK.TabIndex = 3;
        this.OK.Text = "OK";
        this.OK.UseVisualStyleBackColor = true;
        this.OK.Click += this.OK_Click;
        // 
        // IDField
        // 
        this.IDField.Location = new Point(62, 28);
        this.IDField.Name = "IDField";
        this.IDField.Size = new Size(144, 27);
        this.IDField.TabIndex = 4;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new Point(12, 98);
        this.label3.Name = "label3";
        this.label3.Size = new Size(292, 20);
        this.label3.TabIndex = 5;
        this.label3.Text = "also overwrites if the new layer isn't empty.";
        // 
        // LayerMover
        // 
        this.AcceptButton = this.OK;
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.Cancel;
        this.ClientSize = new Size(315, 184);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.IDField);
        this.Controls.Add(this.OK);
        this.Controls.Add(this.Cancel);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "LayerMover";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Move Layer";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label label1;
    private Label label2;
    private Button Cancel;
    private Button OK;
    private TextBox IDField;
    private Label label3;
}