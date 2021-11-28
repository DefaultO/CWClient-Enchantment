using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CWClient_Enchantment_WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Center picturebox if possible. Looks better this way.
            pictureBox1.Location = new Point((this.Width / 2) - (pictureBox1.Width / 2), (this.Height / 2) - (pictureBox1.Height / 2));
            // Create a picture we will draw on and which we later put into the picturebox.
            var bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            string lobbyData = "";
            using (FileStream fs = new FileStream(@"D:\CWClient.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    lobbyData = sr.ReadToEnd();
                }
            }

            // I had to replace the dots with actual commas. The float parse didn't work without this.
            lobbyData = lobbyData.Replace(".", ",");
            // Split newlines to create a player list.
            string[] playerData = lobbyData.Split("\n".ToCharArray());

            foreach(string player in playerData)
            {
                string[] player_part = player.Split('|');
                // Correct Format. Simple Check.
                if (player_part.Length == 12)
                {
                    try
                    {
                        // If not our teammate
                        if (!(bool.Parse(player_part[10]) == bool.Parse(player_part[11])))
                        {
                            // If z > 0, the Enemy is in front of the camera.
                            if (float.Parse(player_part[6]) > 0f)
                            {
                                float height = 1.5f * Math.Abs(float.Parse(player_part[8]) - float.Parse(player_part[5]));
                                float width = height;

                                DrawRectangle((int)(float.Parse(player_part[7]) - (width / 2)), bitmap.Height - (int)float.Parse(player_part[8]), (int)width, (int)height, Color.Red, 2, bitmap);
                            }
                        }
                    }
                    catch
                    {
                        // Do Nothing
                    }
                    
                }
            }

            DrawCrosshair(bitmap);
            pictureBox1.Image = bitmap;
        }

        private void DrawRectangle(int x, int y, int width, int height, Color color, float thickness, Bitmap bmp)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawRectangle(new Pen(color, thickness), new Rectangle(x, y, width, height));
            }
        }

        private void DrawLine(Point A, Point B, Color Color, float Thickness, Bitmap Bmp)
        {
            using (Graphics g = Graphics.FromImage(Bmp))
            {
                g.DrawLine(new Pen(Color, Thickness), A, B);
            }
        }

        private void DrawCrosshair(Bitmap bmp)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawLine(new Pen(Color.White, 2), new Point((bmp.Width / 2) - 10, (bmp.Height / 2) - 10), new Point((bmp.Width / 2) + 10, (bmp.Height / 2) + 10));
                g.DrawLine(new Pen(Color.White, 2), new Point((bmp.Width / 2) + 10, (bmp.Height / 2) - 10), new Point((bmp.Width / 2) - 10, (bmp.Height / 2) + 10));
            }
        }
    }
}
