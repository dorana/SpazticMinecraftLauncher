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
    public partial class QuickstartEditor : Form
    {
        MainForm initialCaller = null;
        public QuickstartEditor(MainForm caller)
        {
            InitializeComponent();
            initialCaller = caller;
            textBox1.Text = initialCaller.settings.quickStartUrl;
        }

        private void QuickstartEditor_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ApplyChanges();
            }
        }

        private void ApplyChanges()
        {
            initialCaller.settings.quickStartUrl = textBox1.Text;
            EpicSettings.Create(initialCaller.settings, MainForm.settingsPath);
            this.Close();
        }
    }
}
