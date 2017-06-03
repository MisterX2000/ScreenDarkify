using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScreenDarkify.Properties;
using static Gamma;

namespace ScreenDarkify
{
    public class Tray : ApplicationContext
    {
        private NotifyIcon trayIcon;

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
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            SetGamma(128);

            Application.Exit();
        }

        void ChangeGamma(object sender, EventArgs e)
        {
            var gamma = 128;
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
            trayIcon.Visible = false;
            SetGamma(128);
        }
    }
}
