﻿namespace PicoMapper.Forms;

partial class RemoveTile
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoveTile));
        this.Cancel = new Button();
        this.Remove = new Button();
        this.TileIDField = new TextBox();
        this.label1 = new Label();
        this.SuspendLayout();
        // 
        // Cancel
        // 
        this.Cancel.Location = new Point(120, 95);
        this.Cancel.Margin = new Padding(3, 4, 3, 4);
        this.Cancel.Name = "Cancel";
        this.Cancel.Size = new Size(86, 31);
        this.Cancel.TabIndex = 0;
        this.Cancel.Text = "Cancel";
        this.Cancel.UseVisualStyleBackColor = true;
        // 
        // Remove
        // 
        this.Remove.Location = new Point(14, 95);
        this.Remove.Margin = new Padding(3, 4, 3, 4);
        this.Remove.Name = "Remove";
        this.Remove.Size = new Size(86, 31);
        this.Remove.TabIndex = 1;
        this.Remove.Text = "Remove";
        this.Remove.UseVisualStyleBackColor = true;
        this.Remove.Click += this.Remove_Click;
        // 
        // TileIDField
        // 
        this.TileIDField.Location = new Point(64, 39);
        this.TileIDField.Margin = new Padding(3, 4, 3, 4);
        this.TileIDField.Name = "TileIDField";
        this.TileIDField.Size = new Size(141, 27);
        this.TileIDField.TabIndex = 2;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(26, 43);
        this.label1.Name = "label1";
        this.label1.Size = new Size(24, 20);
        this.label1.TabIndex = 3;
        this.label1.Text = "ID";
        // 
        // RemoveTile
        // 
        this.AcceptButton = this.Remove;
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.CancelButton = this.Cancel;
        this.ClientSize = new Size(221, 155);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.TileIDField);
        this.Controls.Add(this.Remove);
        this.Controls.Add(this.Cancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.Margin = new Padding(3, 4, 3, 4);
        this.MaximizeBox = false;
        this.Name = "RemoveTile";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Text = "Remove Tile";
        this.TopMost = true;
        this.FormClosed += this.RemoveTile_FormClosed;
        this.Load += this.RemoveTile_Load;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Button Cancel;
    private Button Remove;
    private TextBox TileIDField;
    private Label label1;
}