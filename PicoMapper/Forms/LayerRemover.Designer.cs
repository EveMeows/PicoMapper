namespace PicoMapper.Forms;

partial class LayerRemover
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
        this.IDField = new TextBox();
        this.Cancel = new Button();
        this.OK = new Button();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(18, 34);
        this.label1.Name = "label1";
        this.label1.Size = new Size(44, 20);
        this.label1.TabIndex = 0;
        this.label1.Text = "Layer";
        // 
        // IDField
        // 
        this.IDField.Location = new Point(68, 31);
        this.IDField.Name = "IDField";
        this.IDField.Size = new Size(125, 27);
        this.IDField.TabIndex = 1;
        // 
        // Cancel
        // 
        this.Cancel.Location = new Point(112, 115);
        this.Cancel.Name = "Cancel";
        this.Cancel.Size = new Size(81, 29);
        this.Cancel.TabIndex = 2;
        this.Cancel.Text = "Cancel";
        this.Cancel.UseVisualStyleBackColor = true;
        // 
        // OK
        // 
        this.OK.Location = new Point(12, 115);
        this.OK.Name = "OK";
        this.OK.Size = new Size(75, 29);
        this.OK.TabIndex = 3;
        this.OK.Text = "OK";
        this.OK.UseVisualStyleBackColor = true;
        this.OK.Click += this.OK_Click;
        // 
        // LayerRemover
        // 
        this.AcceptButton = this.OK;
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.Cancel;
        this.ClientSize = new Size(215, 157);
        this.Controls.Add(this.OK);
        this.Controls.Add(this.Cancel);
        this.Controls.Add(this.IDField);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "LayerRemover";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Remove Layer";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox IDField;
    private Button Cancel;
    private Button OK;
}