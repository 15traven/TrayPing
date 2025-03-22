using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace TrayPing
{
    class Program {
        private static NotifyIcon trayIcon;

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            trayIcon = new NotifyIcon();
            CreateTextIcon("15");
            trayIcon.Visible = true;

            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            trayIcon.ContextMenuStrip.Items.Add("Quit", null, (s, e) => Application.Exit());

            Application.Run();
        }

        static void CreateTextIcon(string text) 
        {
            Font font = new Font("TTRounds", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
            Graphics graphics = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            graphics.Clear(Color.Transparent);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            graphics.DrawString(text, font, brush, -4, -2);
           
            hIcon = bitmapText.GetHicon();
            trayIcon.Icon = Icon.FromHandle(hIcon);
        }
    } 
}