namespace FinalClientTemiz
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
            this.IpLabel = new System.Windows.Forms.Label();
            this.IpTextBox = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.EnterButton = new System.Windows.Forms.Button();
            this.RockButton = new System.Windows.Forms.Button();
            this.PaperButton = new System.Windows.Forms.Button();
            this.ScissorsButton = new System.Windows.Forms.Button();
            this.LeaveTheGameButton = new System.Windows.Forms.Button();
            this.GAMElabel = new System.Windows.Forms.Label();
            this.GameRichTextBox = new System.Windows.Forms.RichTextBox();
            this.UPDATEDlabel = new System.Windows.Forms.Label();
            this.UpdatesRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // IpLabel
            // 
            this.IpLabel.AutoSize = true;
            this.IpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.IpLabel.Location = new System.Drawing.Point(49, 46);
            this.IpLabel.Name = "IpLabel";
            this.IpLabel.Size = new System.Drawing.Size(36, 25);
            this.IpLabel.TabIndex = 1;
            this.IpLabel.Text = "IP:";
            // 
            // IpTextBox
            // 
            this.IpTextBox.Location = new System.Drawing.Point(100, 51);
            this.IpTextBox.Name = "IpTextBox";
            this.IpTextBox.Size = new System.Drawing.Size(137, 20);
            this.IpTextBox.TabIndex = 3;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.PortLabel.Location = new System.Drawing.Point(260, 45);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(53, 25);
            this.PortLabel.TabIndex = 4;
            this.PortLabel.Text = "Port:";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(319, 46);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(137, 20);
            this.PortTextBox.TabIndex = 5;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(487, 46);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(137, 24);
            this.ConnectButton.TabIndex = 6;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.NameLabel.Location = new System.Drawing.Point(49, 120);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(224, 25);
            this.NameLabel.TabIndex = 7;
            this.NameLabel.Text = "Please enter your name:";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Enabled = false;
            this.NameTextBox.Location = new System.Drawing.Point(297, 125);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(137, 20);
            this.NameTextBox.TabIndex = 8;
            // 
            // EnterButton
            // 
            this.EnterButton.Enabled = false;
            this.EnterButton.Location = new System.Drawing.Point(464, 125);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(137, 24);
            this.EnterButton.TabIndex = 9;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // RockButton
            // 
            this.RockButton.Enabled = false;
            this.RockButton.Location = new System.Drawing.Point(54, 190);
            this.RockButton.Name = "RockButton";
            this.RockButton.Size = new System.Drawing.Size(137, 24);
            this.RockButton.TabIndex = 13;
            this.RockButton.Text = "Rock";
            this.RockButton.UseVisualStyleBackColor = true;
            this.RockButton.Click += new System.EventHandler(this.RockButton_Click);
            // 
            // PaperButton
            // 
            this.PaperButton.Enabled = false;
            this.PaperButton.Location = new System.Drawing.Point(229, 190);
            this.PaperButton.Name = "PaperButton";
            this.PaperButton.Size = new System.Drawing.Size(137, 24);
            this.PaperButton.TabIndex = 14;
            this.PaperButton.Text = "Paper";
            this.PaperButton.UseVisualStyleBackColor = true;
            this.PaperButton.Click += new System.EventHandler(this.PaperButton_Click);
            // 
            // ScissorsButton
            // 
            this.ScissorsButton.Enabled = false;
            this.ScissorsButton.Location = new System.Drawing.Point(388, 190);
            this.ScissorsButton.Name = "ScissorsButton";
            this.ScissorsButton.Size = new System.Drawing.Size(137, 24);
            this.ScissorsButton.TabIndex = 15;
            this.ScissorsButton.Text = "Scissors";
            this.ScissorsButton.UseVisualStyleBackColor = true;
            this.ScissorsButton.Click += new System.EventHandler(this.ScissorsButton_Click);
            // 
            // LeaveTheGameButton
            // 
            this.LeaveTheGameButton.Enabled = false;
            this.LeaveTheGameButton.Location = new System.Drawing.Point(544, 190);
            this.LeaveTheGameButton.Name = "LeaveTheGameButton";
            this.LeaveTheGameButton.Size = new System.Drawing.Size(137, 24);
            this.LeaveTheGameButton.TabIndex = 16;
            this.LeaveTheGameButton.Text = "Leave The Game";
            this.LeaveTheGameButton.UseVisualStyleBackColor = true;
            this.LeaveTheGameButton.Click += new System.EventHandler(this.LeaveTheGameButton_Click);
            // 
            // GAMElabel
            // 
            this.GAMElabel.AutoSize = true;
            this.GAMElabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.GAMElabel.Location = new System.Drawing.Point(95, 241);
            this.GAMElabel.Name = "GAMElabel";
            this.GAMElabel.Size = new System.Drawing.Size(71, 25);
            this.GAMElabel.TabIndex = 17;
            this.GAMElabel.Text = "GAME";
            // 
            // GameRichTextBox
            // 
            this.GameRichTextBox.Location = new System.Drawing.Point(32, 279);
            this.GameRichTextBox.Name = "GameRichTextBox";
            this.GameRichTextBox.Size = new System.Drawing.Size(305, 137);
            this.GameRichTextBox.TabIndex = 18;
            this.GameRichTextBox.Text = "";
            // 
            // UPDATEDlabel
            // 
            this.UPDATEDlabel.AutoSize = true;
            this.UPDATEDlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.UPDATEDlabel.Location = new System.Drawing.Point(471, 241);
            this.UPDATEDlabel.Name = "UPDATEDlabel";
            this.UPDATEDlabel.Size = new System.Drawing.Size(107, 25);
            this.UPDATEDlabel.TabIndex = 19;
            this.UPDATEDlabel.Text = "UPDATES";
            // 
            // UpdatesRichTextBox
            // 
            this.UpdatesRichTextBox.Location = new System.Drawing.Point(362, 279);
            this.UpdatesRichTextBox.Name = "UpdatesRichTextBox";
            this.UpdatesRichTextBox.Size = new System.Drawing.Size(319, 137);
            this.UpdatesRichTextBox.TabIndex = 20;
            this.UpdatesRichTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UpdatesRichTextBox);
            this.Controls.Add(this.UPDATEDlabel);
            this.Controls.Add(this.GameRichTextBox);
            this.Controls.Add(this.GAMElabel);
            this.Controls.Add(this.LeaveTheGameButton);
            this.Controls.Add(this.ScissorsButton);
            this.Controls.Add(this.PaperButton);
            this.Controls.Add(this.RockButton);
            this.Controls.Add(this.EnterButton);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.IpTextBox);
            this.Controls.Add(this.IpLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IpLabel;
        private System.Windows.Forms.TextBox IpTextBox;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.Button RockButton;
        private System.Windows.Forms.Button PaperButton;
        private System.Windows.Forms.Button ScissorsButton;
        private System.Windows.Forms.Button LeaveTheGameButton;
        private System.Windows.Forms.Label GAMElabel;
        private System.Windows.Forms.RichTextBox GameRichTextBox;
        private System.Windows.Forms.Label UPDATEDlabel;
        private System.Windows.Forms.RichTextBox UpdatesRichTextBox;
    }
}

