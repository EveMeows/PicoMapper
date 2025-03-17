namespace PicoMapper.Forms;

partial class AddTile
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTile));
        this.Cancel = new Button();
        this.TileAdd = new Button();
        this.label1 = new Label();
        this.label2 = new Label();
        this.IDField = new TextBox();
        this.PathText = new TextBox();
        this.PathBrowser = new Button();
        this.PathChooser = new OpenFileDialog();
        this.SuspendLayout();
        // 
        // Cancel
        // 
        this.Cancel.Location = new Point(149, 110);
        this.Cancel.Name = "Cancel";
        this.Cancel.Size = new Size(75, 23);
        this.Cancel.TabIndex = 0;
        this.Cancel.Text = "Cancel";
        this.Cancel.UseVisualStyleBackColor = true;
        // 
        // TileAdd
        // 
        this.TileAdd.Location = new Point(12, 110);
        this.TileAdd.Name = "TileAdd";
        this.TileAdd.Size = new Size(75, 23);
        this.TileAdd.TabIndex = 1;
        this.TileAdd.Text = "Add Tile";
        this.TileAdd.UseVisualStyleBackColor = true;
        this.TileAdd.Click += this.TileAdd_Click;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 16);
        this.label1.Name = "label1";
        this.label1.Size = new Size(40, 15);
        this.label1.TabIndex = 2;
        this.label1.Text = "Tile ID";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new Point(12, 53);
        this.label2.Name = "label2";
        this.label2.Size = new Size(53, 15);
        this.label2.TabIndex = 3;
        this.label2.Text = "Tile Path";
        // 
        // IDField
        // 
        this.IDField.Location = new Point(70, 13);
        this.IDField.Name = "IDField";
        this.IDField.PlaceholderText = "ID...";
        this.IDField.Size = new Size(149, 23);
        this.IDField.TabIndex = 4;
        // 
        // PathText
        // 
        this.PathText.AllowDrop = true;
        this.PathText.Location = new Point(70, 50);
        this.PathText.Name = "PathText";
        this.PathText.PlaceholderText = "Path...";
        this.PathText.Size = new Size(128, 23);
        this.PathText.TabIndex = 5;
        // 
        // PathBrowser
        // 
        this.PathBrowser.Location = new Point(195, 50);
        this.PathBrowser.Name = "PathBrowser";
        this.PathBrowser.Size = new Size(24, 23);
        this.PathBrowser.TabIndex = 6;
        this.PathBrowser.Text = "...";
        this.PathBrowser.UseVisualStyleBackColor = true;
        this.PathBrowser.Click += this.PathBrowser_Click;
        // 
        // PathChooser
        // 
        this.PathChooser.FileName = "PathChooser";
        // 
        // AddTile
        // 
        this.AcceptButton = this.TileAdd;
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.Cancel;
        this.ClientSize = new Size(236, 150);
        this.Controls.Add(this.PathBrowser);
        this.Controls.Add(this.PathText);
        this.Controls.Add(this.IDField);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.TileAdd);
        this.Controls.Add(this.Cancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.MaximizeBox = false;
        this.Name = "AddTile";
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "New Tile";
        this.TopMost = true;
        this.FormClosed += this.AddTile_FormClosed;
        this.Load += this.AddTile_Load;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Button Cancel;
    private Button TileAdd;
    private Label label1;
    private Label label2;
    private TextBox IDField;
    private TextBox PathText;
    private Button PathBrowser;
    private OpenFileDialog PathChooser;
}