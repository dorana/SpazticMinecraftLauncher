using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Epicserver_Minecraft_Installer
{
    public partial class JVMEditor : Form
    {
        MainForm initialCaller = null;
        public JVMEditor(MainForm caller)
        {
            InitializeComponent();
            initialCaller = caller;
            textBox1.Text = initialCaller.settings.launcherSettings.jvmArguments;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void ApplyChanges()
        {
            initialCaller.settings.launcherSettings.jvmArguments = textBox1.Text;
            EpicSettings.Create(initialCaller.settings, MainForm.settingsPath);
            this.Close();
        }

    }
}
