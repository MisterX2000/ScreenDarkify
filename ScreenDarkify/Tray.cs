using System;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;
using ScreenDarkify.Properties;
using static Gamma;

namespace ScreenDarkify
{
    public class Tray : ApplicationContext
    {
        NotifyIcon trayIcon;
        int gamma = 128;
        bool SaveOnExit = true;

        public Tray()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon
            {
                Icon = Resources.AppIcon,
                ContextMenuStrip = new ContextMenuStrip(), 
                Visible = true
            };

            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("100%", Resources.brightness_7, ChangeGamma));
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("75%", Resources.brightness_6, ChangeGamma));
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("50%", Resources.brightness_5, ChangeGamma));
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("25%", Resources.brightness_3, ChangeGamma));
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("0%", Resources.brightness_1, ChangeGamma));
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", Resources.close, Exit));

            Application.ApplicationExit += OnApplicationExit;

            LoadSettings();
        }

        void LoadSettings()
        {
            if (!File.Exists("Config.ini"))
            {
                SaveSettings();
                return;
            }

            var parser = new FileIniDataParser();
            var data = new IniData();

            data = parser.ReadFile("Config.ini");
            if (!int.TryParse(data["Config"]["Brightness"], out gamma))
            {
                MessageBox.Show("Incorrect Brightness value in config!\nResetting config.", "Could not load config",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                gamma = 128;
                File.Delete("Config.ini");
                SaveSettings();
                return;
            }
            if (!bool.TryParse(data["Config"]["SaveOnExit"], out SaveOnExit))
            {
                MessageBox.Show("Incorrect SaveOnExit value in config!\nResetting config.", "Could not load config",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SaveOnExit = true;
                File.Delete("Config.ini");
                SaveSettings();
                return;
            }
            SetGamma(gamma);
        }

        void SaveSettings()
        {
            var parser = new FileIniDataParser();
            var data = new IniData();
            data["Config"]["Brightness"] = gamma.ToString();
            data["Config"]["SaveOnExit"] = SaveOnExit.ToString();
            parser.WriteFile("Config.ini", data);
        }

        void Exit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void ChangeGamma(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).Text)
            {
                case "100%":
                    gamma = 128;
                    break;
                case "75%":
                    gamma = 96;
                    break;
                case "50%":
                    gamma = 64;
                    break;
                case "25%":
                    gamma = 32;
                    break;
                case "0%":
                    gamma = 1;
                    break;
            }

            SetGamma(gamma);
        }

        void OnApplicationExit(object sender, EventArgs e)
        {
            if (SaveOnExit)
                SaveSettings();

            trayIcon.Visible = false;
            SetGamma(128);
        }
    }
}
