namespace PicoMapper.Forms;

partial class Creator
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Creator));
        this.TileX = new TextBox();
        this.CancelCreation = new Button();
        this.CreateMap = new Button();
        this.label1 = new Label();
        this.TileY = new TextBox();
        this.label2 = new Label();
        this.label3 = new Label();
        this.MapY = new TextBox();
        this.label4 = new Label();
        this.MapX = new TextBox();
        this.SuspendLayout();
        // 
        // TileX
        // 
        this.TileX.Location = new Point(77, 33);
        this.TileX.Name = "TileX";
        this.TileX.Size = new Size(26, 23);
        this.TileX.TabIndex = 1;
        this.TileX.TextChanged += this.textBox2_TextChanged;
        // 
        // CancelCreation
        // 
        this.CancelCreation.Location = new Point(129, 144);
        this.CancelCreation.Name = "CancelCreation";
        this.CancelCreation.Size = new Size(52, 23);
        this.CancelCreation.TabIndex = 2;
        this.CancelCreation.Text = "Cancel";
        this.CancelCreation.UseVisualStyleBackColor = true;
        // 
        // CreateMap
        // 
        this.CreateMap.Location = new Point(50, 144);
        this.CreateMap.Name = "CreateMap";
        this.CreateMap.Size = new Size(53, 23);
        this.CreateMap.TabIndex = 3;
        this.CreateMap.Text = "Ok";
        this.CreateMap.UseVisualStyleBackColor = true;
        this.CreateMap.Click += this.CreateMap_Click;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(109, 36);
        this.label1.Name = "label1";
        this.label1.Size = new Size(14, 15);
        this.label1.TabIndex = 4;
        this.label1.Text = "X";
        // 
        // TileY
        // 
        this.TileY.Location = new Point(129, 33);
        this.TileY.Name = "TileY";
        this.TileY.Size = new Size(26, 23);
        this.TileY.TabIndex = 5;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new Point(91, 9);
        this.label2.Name = "label2";
        this.label2.Size = new Size(48, 15);
        this.label2.TabIndex = 6;
        this.label2.Text = "Tile Size";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new Point(91, 71);
        this.label3.Name = "label3";
        this.label3.Size = new Size(54, 15);
        this.label3.TabIndex = 10;
        this.label3.Text = "Map Size";
        // 
        // MapY
        // 
        this.MapY.Location = new Point(129, 96);
        this.MapY.Name = "MapY";
        this.MapY.Size = new Size(26, 23);
        this.MapY.TabIndex = 9;
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new Point(109, 99);
        this.label4.Name = "label4";
        this.label4.Size = new Size(14, 15);
        this.label4.TabIndex = 8;
        this.label4.Text = "X";
        // 
        // MapX
        // 
        this.MapX.Location = new Point(77, 96);
        this.MapX.Name = "MapX";
        this.MapX.Size = new Size(26, 23);
        this.MapX.TabIndex = 7;
        // 
        // Creator
        // 
        this.AcceptButton = this.CreateMap;
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.CancelCreation;
        this.ClientSize = new Size(230, 198);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.MapY);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.MapX);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.TileY);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.CreateMap);
        this.Controls.Add(this.CancelCreation);
        this.Controls.Add(this.TileX);
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.MaximizeBox = false;
        this.Name = "Creator";
        this.ShowInTaskbar = false;
        this.Text = "New Map";
        this.TopMost = true;
        this.FormClosed += this.Creator_FormClosed;
        this.Load += this.Creator_Load;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private TextBox TileX;
    private Button CancelCreation;
    private Button CreateMap;
    private Label label1;
    private TextBox TileY;
    private Label label2;
    private Label label3;
    private TextBox MapY;
    private Label label4;
    private TextBox MapX;
}