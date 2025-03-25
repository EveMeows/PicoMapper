namespace PicoMapper.Forms;

partial class LayerSwapper
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerSwapper));
        this.SwapLayers = new Button();
        this.Cancel = new Button();
        this.label1 = new Label();
        this.FirstLayer = new TextBox();
        this.SecondLayer = new TextBox();
        this.label2 = new Label();
        this.SuspendLayout();
        // 
        // SwapLayers
        // 
        this.SwapLayers.Location = new Point(21, 157);
        this.SwapLayers.Margin = new Padding(3, 4, 3, 4);
        this.SwapLayers.Name = "SwapLayers";
        this.SwapLayers.Size = new Size(86, 31);
        this.SwapLayers.TabIndex = 0;
        this.SwapLayers.Text = "OK";
        this.SwapLayers.UseVisualStyleBackColor = true;
        this.SwapLayers.Click += this.SwapLayers_Click;
        // 
        // Cancel
        // 
        this.Cancel.Location = new Point(142, 157);
        this.Cancel.Margin = new Padding(3, 4, 3, 4);
        this.Cancel.Name = "Cancel";
        this.Cancel.Size = new Size(86, 31);
        this.Cancel.TabIndex = 1;
        this.Cancel.Text = "Cancel";
        this.Cancel.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(18, 20);
        this.label1.Name = "label1";
        this.label1.Size = new Size(79, 20);
        this.label1.TabIndex = 2;
        this.label1.Text = "Back Layer";
        // 
        // FirstLayer
        // 
        this.FirstLayer.Location = new Point(113, 16);
        this.FirstLayer.Margin = new Padding(3, 4, 3, 4);
        this.FirstLayer.Name = "FirstLayer";
        this.FirstLayer.Size = new Size(114, 27);
        this.FirstLayer.TabIndex = 3;
        // 
        // SecondLayer
        // 
        this.SecondLayer.Location = new Point(113, 81);
        this.SecondLayer.Margin = new Padding(3, 4, 3, 4);
        this.SecondLayer.Name = "SecondLayer";
        this.SecondLayer.Size = new Size(114, 27);
        this.SecondLayer.TabIndex = 5;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new Point(18, 85);
        this.label2.Name = "label2";
        this.label2.Size = new Size(82, 20);
        this.label2.TabIndex = 4;
        this.label2.Text = "Front Layer";
        // 
        // LayerSwapper
        // 
        this.AcceptButton = this.SwapLayers;
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.Cancel;
        this.ClientSize = new Size(246, 207);
        this.Controls.Add(this.SecondLayer);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.FirstLayer);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.Cancel);
        this.Controls.Add(this.SwapLayers);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.Margin = new Padding(3, 4, 3, 4);
        this.MaximizeBox = false;
        this.Name = "LayerSwapper";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "LayerSwapper";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Button SwapLayers;
    private Button Cancel;
    private Label label1;
    private TextBox FirstLayer;
    private TextBox SecondLayer;
    private Label label2;
}