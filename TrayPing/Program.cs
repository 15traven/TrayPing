using System;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;

namespace TrayPing
{
    class Program {
        private static NotifyIcon trayIcon;

        static void Main()
        {
            trayIcon = new NotifyIcon
            {
                ContextMenuStrip = new TrayMenu(),
                Visible = true
            };
            
            trayIcon.ContextMenuStrip.Items.Add("Preferences", null, (s, e) => new PreferencesForm().Show());
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            trayIcon.ContextMenuStrip.Items.Add("About", null, ShowAbout);
            trayIcon.ContextMenuStrip.Items.Add("Quit", null, (s, e) => Application.Exit());

            Autolaunch.Register();

            _ = UpdatePingLoop();

            Application.Run();
        }

        static void CreateTextIcon(string text) 
        {
            Font font = new("TTRounds", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.White);
            Bitmap bitmapText = new(16, 16);
            Graphics graphics = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(text, font, brush, -4, -2);
           
            hIcon = bitmapText.GetHicon();
            trayIcon.Icon = Icon.FromHandle(hIcon);
        }

        static async Task UpdatePingLoop()
        {
            while (true)
            {
                long ping = GetPing("www.google.com");
                CreateTextIcon(ping >= 0 ? ping.ToString() : "Er" );
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }


        static long GetPing(string host)
        {
            try
            {
                Ping ping = new();
                PingReply reply = ping.Send(host);
                if (reply.Status == IPStatus.Success)
                {
                    return reply.RoundtripTime;
                }
            }
            catch
            {
                return -1;
            }

            return -1;
        }

        static void ShowAbout(object sender, EventArgs e)
        {
            MessageBox.Show(
                "TrayPing v.1.0\nPing status tray app",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    } 
}
