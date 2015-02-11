/*
 * FileName: VersionSelect.cs
 * Author: Wiebe Geertsma
 * Date: 9-8-2013
 * Copyright (C) 2013 Positive Computers
 * 
 * This file is subject to the terms and conditions defined in
 * file 'GPL.txt', which is part of this source code package.
 */

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
    public partial class VersionSelect : Form
    {
        MainForm initialCaller = null;
        public VersionSelect(MainForm caller)
        {
            InitializeComponent();
            initialCaller = caller;
            textBox1.Text = initialCaller.settings.launcherSettings.minecraftVersion;
            textBox2.Text = initialCaller.settings.launcherSettings.forgeVersion;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initialCaller.settings.launcherSettings.minecraftVersion = textBox1.Text;
            initialCaller.settings.launcherSettings.forgeVersion = textBox2.Text;
            EpicSettings.Create(initialCaller.settings, MainForm.settingsPath);
            this.Close();
        }
    }
}
