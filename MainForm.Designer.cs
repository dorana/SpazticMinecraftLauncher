namespace Epicserver_Minecraft_Installer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWhenMinecraftStartsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automaticallyCheckForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchWithForge = new System.Windows.Forms.ToolStripMenuItem();
            this.showConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeQuickstartServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.versionOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jVMOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.file = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btnChangeVersion = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.usernameInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.buttonStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.SizingGrip = false;
            // 
            // status
            // 
            this.status.Name = "status";
            resources.ApplyResources(this.status, "status");
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            resources.ApplyResources(this.statusLabel, "statusLabel");
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // menuStrip
            // 
            this.menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Name = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeWhenMinecraftStartsToolStripMenuItem,
            this.automaticallyCheckForUpdatesToolStripMenuItem,
            this.launchWithForge,
            this.showConsoleToolStripMenuItem,
            this.changeQuickstartServerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.versionOptionsToolStripMenuItem,
            this.jVMOptionsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
            // 
            // closeWhenMinecraftStartsToolStripMenuItem
            // 
            this.closeWhenMinecraftStartsToolStripMenuItem.Checked = true;
            this.closeWhenMinecraftStartsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeWhenMinecraftStartsToolStripMenuItem.Name = "closeWhenMinecraftStartsToolStripMenuItem";
            resources.ApplyResources(this.closeWhenMinecraftStartsToolStripMenuItem, "closeWhenMinecraftStartsToolStripMenuItem");
            this.closeWhenMinecraftStartsToolStripMenuItem.Click += new System.EventHandler(this.closeWhenMinecraftStartsToolStripMenuItem_Click);
            // 
            // automaticallyCheckForUpdatesToolStripMenuItem
            // 
            resources.ApplyResources(this.automaticallyCheckForUpdatesToolStripMenuItem, "automaticallyCheckForUpdatesToolStripMenuItem");
            this.automaticallyCheckForUpdatesToolStripMenuItem.Name = "automaticallyCheckForUpdatesToolStripMenuItem";
            this.automaticallyCheckForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.automaticallyCheckForUpdatesToolStripMenuItem_Click);
            // 
            // launchWithForge
            // 
            this.launchWithForge.Name = "launchWithForge";
            resources.ApplyResources(this.launchWithForge, "launchWithForge");
            this.launchWithForge.Click += new System.EventHandler(this.launchWithForge_Click);
            // 
            // showConsoleToolStripMenuItem
            // 
            this.showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
            resources.ApplyResources(this.showConsoleToolStripMenuItem, "showConsoleToolStripMenuItem");
            this.showConsoleToolStripMenuItem.Click += new System.EventHandler(this.showConsoleToolStripMenuItem_Click);
            // 
            // changeQuickstartServerToolStripMenuItem
            // 
            this.changeQuickstartServerToolStripMenuItem.Name = "changeQuickstartServerToolStripMenuItem";
            resources.ApplyResources(this.changeQuickstartServerToolStripMenuItem, "changeQuickstartServerToolStripMenuItem");
            this.changeQuickstartServerToolStripMenuItem.Click += new System.EventHandler(this.changeQuickstartServerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // versionOptionsToolStripMenuItem
            // 
            this.versionOptionsToolStripMenuItem.Name = "versionOptionsToolStripMenuItem";
            resources.ApplyResources(this.versionOptionsToolStripMenuItem, "versionOptionsToolStripMenuItem");
            this.versionOptionsToolStripMenuItem.Click += new System.EventHandler(this.versionOptionsToolStripMenuItem_Click);
            // 
            // jVMOptionsToolStripMenuItem
            // 
            this.jVMOptionsToolStripMenuItem.Name = "jVMOptionsToolStripMenuItem";
            resources.ApplyResources(this.jVMOptionsToolStripMenuItem, "jVMOptionsToolStripMenuItem");
            this.jVMOptionsToolStripMenuItem.Click += new System.EventHandler(this.jVMOptionsToolStripMenuItem_Click);
            // 
            // file
            // 
            this.file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exit});
            this.file.Name = "file";
            resources.ApplyResources(this.file, "file");
            // 
            // exit
            // 
            this.exit.Name = "exit";
            resources.ApplyResources(this.exit, "exit");
            // 
            // buttonStrip
            // 
            resources.ApplyResources(this.buttonStrip, "buttonStrip");
            this.buttonStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton1,
            this.btnChangeVersion,
            this.toolStripButton3,
            this.toolStripButton2});
            this.buttonStrip.Name = "buttonStrip";
            // 
            // toolStripButton4
            // 
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.Image = global::Epicserver_Minecraft_Installer.Properties.Resources.minecraftIco;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Image = global::Epicserver_Minecraft_Installer.Properties.Resources.minecraftIco;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnChangeVersion
            // 
            resources.ApplyResources(this.btnChangeVersion, "btnChangeVersion");
            this.btnChangeVersion.BackColor = System.Drawing.SystemColors.Control;
            this.btnChangeVersion.Image = global::Epicserver_Minecraft_Installer.Properties.Resources.EpicMap;
            this.btnChangeVersion.Name = "btnChangeVersion";
            this.btnChangeVersion.Click += new System.EventHandler(this.btnChangeVersion_Click);
            // 
            // toolStripButton3
            // 
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripButton3.Image = global::Epicserver_Minecraft_Installer.Properties.Resources.Repair;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripButton2.Image = global::Epicserver_Minecraft_Installer.Properties.Resources.CheckUpdates;
            this.toolStripButton2.Name = "toolStripButton2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowNavigation = false;
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            resources.ApplyResources(this.webBrowser1, "webBrowser1");
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.TabStop = false;
            this.webBrowser1.Url = new System.Uri("http://www.epicserver.nl/newsfeed.html", System.UriKind.Absolute);
            // 
            // passwordInput
            // 
            resources.ApplyResources(this.passwordInput, "passwordInput");
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordInput_KeyPress);
            // 
            // usernameInput
            // 
            resources.ApplyResources(this.usernameInput, "usernameInput");
            this.usernameInput.Name = "usernameInput";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.usernameInput);
            this.Controls.Add(this.passwordInput);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStrip);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.buttonStrip.ResumeLayout(false);
            this.buttonStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.StatusStrip statusStrip;
        public System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem file;
        private System.Windows.Forms.ToolStripMenuItem exit;
        private System.Windows.Forms.ToolStrip buttonStrip;
        private System.Windows.Forms.ToolStripButton btnChangeVersion;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem automaticallyCheckForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.TextBox usernameInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem launchWithForge;
        private System.Windows.Forms.ToolStripMenuItem showConsoleToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem closeWhenMinecraftStartsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeQuickstartServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem versionOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jVMOptionsToolStripMenuItem;
    }
}

