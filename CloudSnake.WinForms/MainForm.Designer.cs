namespace CloudSnake.WinForms;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.mainMenuPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.joinGameButton = new System.Windows.Forms.Button();
            this.newGameButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.mainMenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuPanel
            // 
            this.mainMenuPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mainMenuPanel.BackColor = System.Drawing.Color.White;
            this.mainMenuPanel.Controls.Add(this.label2);
            this.mainMenuPanel.Controls.Add(this.label1);
            this.mainMenuPanel.Controls.Add(this.pictureBox1);
            this.mainMenuPanel.Controls.Add(this.joinGameButton);
            this.mainMenuPanel.Controls.Add(this.newGameButton);
            this.mainMenuPanel.Location = new System.Drawing.Point(134, 162);
            this.mainMenuPanel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.mainMenuPanel.Name = "mainMenuPanel";
            this.mainMenuPanel.Size = new System.Drawing.Size(620, 209);
            this.mainMenuPanel.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(408, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 85);
            this.label2.TabIndex = 4;
            this.label2.Text = "Snake";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(19, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 85);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cloud";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CloudSnake.WinForms.Properties.Resources.snake;
            this.pictureBox1.Location = new System.Drawing.Point(206, 7);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(199, 143);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // joinGameButton
            // 
            this.joinGameButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(50)))), ((int)(((byte)(102)))));
            this.joinGameButton.ForeColor = System.Drawing.Color.White;
            this.joinGameButton.Location = new System.Drawing.Point(325, 153);
            this.joinGameButton.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.joinGameButton.Name = "joinGameButton";
            this.joinGameButton.Size = new System.Drawing.Size(220, 37);
            this.joinGameButton.TabIndex = 1;
            this.joinGameButton.Text = "JOIN EXISTING GAME";
            this.joinGameButton.UseVisualStyleBackColor = false;
            this.joinGameButton.Click += new System.EventHandler(this.joinGameButton_Click);
            // 
            // newGameButton
            // 
            this.newGameButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(50)))), ((int)(((byte)(102)))));
            this.newGameButton.ForeColor = System.Drawing.Color.White;
            this.newGameButton.Location = new System.Drawing.Point(66, 153);
            this.newGameButton.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.newGameButton.Name = "newGameButton";
            this.newGameButton.Size = new System.Drawing.Size(220, 37);
            this.newGameButton.TabIndex = 0;
            this.newGameButton.Text = "START NEW GAME";
            this.newGameButton.UseVisualStyleBackColor = false;
            this.newGameButton.Click += new System.EventHandler(this.newGameButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.Location = new System.Drawing.Point(6, 511);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(874, 19);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Connecting to the game server...";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(50)))), ((int)(((byte)(102)))));
            this.ClientSize = new System.Drawing.Size(887, 497);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.mainMenuPanel);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "MainForm";
            this.Text = "CloudSnake";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.mainMenuPanel.ResumeLayout(false);
            this.mainMenuPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private Panel mainMenuPanel;
    private Button joinGameButton;
    private Button newGameButton;
    private PictureBox pictureBox1;
    private Label label1;
    private Label label2;
    private Label statusLabel;
}