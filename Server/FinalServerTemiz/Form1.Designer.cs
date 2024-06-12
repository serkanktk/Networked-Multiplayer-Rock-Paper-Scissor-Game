namespace FinalServerTemiz
{
    partial class Form1
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
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LeaderboardRichTextBox = new System.Windows.Forms.RichTextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.gameLabel = new System.Windows.Forms.Label();
            this.GameRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ServerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // PortTextBox
            // 
            this.PortTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.PortTextBox.Location = new System.Drawing.Point(109, 52);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(221, 31);
            this.PortTextBox.TabIndex = 2;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.PortLabel.Location = new System.Drawing.Point(22, 52);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(64, 31);
            this.PortLabel.TabIndex = 3;
            this.PortLabel.Text = "Port";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(353, 49);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(101, 43);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.label2.Location = new System.Drawing.Point(541, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "Leaderboard:";
            // 
            // LeaderboardRichTextBox
            // 
            this.LeaderboardRichTextBox.Location = new System.Drawing.Point(546, 98);
            this.LeaderboardRichTextBox.Name = "LeaderboardRichTextBox";
            this.LeaderboardRichTextBox.Size = new System.Drawing.Size(230, 324);
            this.LeaderboardRichTextBox.TabIndex = 10;
            this.LeaderboardRichTextBox.Text = "";
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.serverLabel.Location = new System.Drawing.Point(293, 140);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(98, 25);
            this.serverLabel.TabIndex = 11;
            this.serverLabel.Text = "SERVER:";
            // 
            // gameLabel
            // 
            this.gameLabel.AutoSize = true;
            this.gameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.gameLabel.Location = new System.Drawing.Point(23, 140);
            this.gameLabel.Name = "gameLabel";
            this.gameLabel.Size = new System.Drawing.Size(77, 25);
            this.gameLabel.TabIndex = 12;
            this.gameLabel.Text = "GAME:";
            // 
            // GameRichTextBox
            // 
            this.GameRichTextBox.Location = new System.Drawing.Point(28, 194);
            this.GameRichTextBox.Name = "GameRichTextBox";
            this.GameRichTextBox.Size = new System.Drawing.Size(184, 184);
            this.GameRichTextBox.TabIndex = 13;
            this.GameRichTextBox.Text = "";
            // 
            // ServerRichTextBox
            // 
            this.ServerRichTextBox.Location = new System.Drawing.Point(298, 194);
            this.ServerRichTextBox.Name = "ServerRichTextBox";
            this.ServerRichTextBox.Size = new System.Drawing.Size(186, 184);
            this.ServerRichTextBox.TabIndex = 14;
            this.ServerRichTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ServerRichTextBox);
            this.Controls.Add(this.GameRichTextBox);
            this.Controls.Add(this.gameLabel);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.LeaderboardRichTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.PortTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox LeaderboardRichTextBox;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label gameLabel;
        private System.Windows.Forms.RichTextBox GameRichTextBox;
        private System.Windows.Forms.RichTextBox ServerRichTextBox;
    }
}

