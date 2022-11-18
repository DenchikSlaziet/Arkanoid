using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArkanaNoNeDota
{
    public partial class Form1 : Form
    {
        IList<PictureBox> Bricks = new List<PictureBox>();
        PictureBox Plate;
        PictureBox Ball;
        float x,y,LocX,LocY;
        bool Reversed;
        Random r = new Random();
        Timer t = new Timer();
        Timer New = new Timer();
        public Form1()
        {
            InitializeComponent();
            t.Interval = 1;
            t.Tick += Tick;
            t.Start();
            New.Interval = 1000;
            New.Tick += NewGame;
        }

        private void NewGame(object sender, EventArgs e)
        {
            CreateLVL();
            New.Stop();
        }

        private void Tick(object sender, EventArgs e)
        {
            LocX += x;
            LocY -= y;
            Ball.Location = new Point((int)(LocX), (int)(LocY));
            if (Ball.Location.X < 0 || Ball.Location.X > Width - 20)
                x = -x;
            if (Ball.Location.Y < 0)
                y = -y;
            if (Ball.Location.Y > Height)
                New.Start();
            int Count=0;
            foreach (PictureBox brick in Bricks)
            {
                Count += brick.BackColor == BackColor ? 1 : 0;
                if (brick.BackColor!=BackColor && brick.Bounds.IntersectsWith(Ball.Bounds))
                {
                    if (Reversed)
                    {
                        y = -y;
                    }
                    if (LocX < brick.Location.X || LocX > brick.Location.X + 100)
                        x = -x;
                    if (LocY-10 < brick.Location.Y)
                        y = -y;
                    Reversed = false;
                    if (int.Parse(brick.Name) == 3)
                    {
                        brick.BackColor = BackColor;
                        continue;
                    }
                    brick.BackColor = colors[int.Parse(brick.Name) + 1];
                    brick.Name = (int.Parse(brick.Name) + 1).ToString();
                }
            }
            if (Count == Bricks.Count) New.Start();
            if (Plate.Bounds.IntersectsWith(Ball.Bounds))
            {
                y = -y;
                x = -(Plate.Location.X + 75 - Ball.Location.X - 10)/10;
                Reversed = true;
            }
            int Pos = Cursor.Position.X - Location.X - 75;
            Plate.Location = new Point(Pos<-149?-149:Pos>Width-17?Width-17:Pos,Height - 85);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateLVL();
        }
        Color[] colors =
        {
            (Color.Red),
            (Color.Yellow),
            (Color.Green),
            (Color.White)
        };
        void CreateLVL()
        {
            Reversed = true;
            for (int i = Bricks.Count-1; i >= 0; i--)
            {
                Controls.Remove(Bricks[i]);
                Bricks[i].Dispose();
                Bricks.RemoveAt(i);
            }
            if(Plate != null) Plate.Dispose();
            Plate = new PictureBox()
            {
                Size = new Size(150, 10),
                Location = new Point(Width / 2 - 75, Height - 65),
                BackColor = Color.Violet,
            };
            if (Ball != null) Ball.Dispose();
            Ball = new PictureBox()
            {
                Size = new Size(20, 20),
                Location = new Point(Width / 2 - 10, Height - 100),
                Image = new Bitmap(Width, Height),
            };
            x = r.Next(-3, 4);
            y = 5;
            LocX = Ball.Location.X;
            LocY = Ball.Location.Y;
            Controls.Add(Ball);
            Graphics.FromImage(Ball.Image).FillEllipse(Brushes.Gray, 5, 5, 10, 10);
            Controls.Add(Plate);
            switch (r.Next(4))
            {
                case 0:
                    for (int i = 0; i < 24; i++)
                    {
                        Bricks.Add(new PictureBox()
                        {
                            Size = new Size(98, 28),
                            BackColor = colors[i / 6],
                            Name = (i / 6).ToString(),
                            Location = new Point(i % 6 * 100, i / 6 * 30 + 100),
                        });;
                        Controls.Add(Bricks[i]);
                    }
                    break;
                case 1:
                    for (int x = 6; x >= 0; x--)
                    {
                        for (int i = 0; i < x; i++)
                        {
                            Bricks.Add(new PictureBox()
                            {
                                Size = new Size(98, 28),
                                BackColor = colors[x/2],
                                Name = (x / 2).ToString(),
                                Location = new Point(i % 6 * 100 + 50 * (6-x), x * 30 + 100),
                            });
                            Controls.Add(Bricks[Bricks.Count-1]);
                        }
                    }
                    break;
                case 2:
                    for (int x = 6; x >= 0; x--)
                    {
                        for (int i = 0; i < x; i++)
                        {
                            Bricks.Add(new PictureBox()
                            {
                                Size = new Size(98, 28),
                                BackColor = colors[x/2],
                                Name = (x / 2).ToString(),
                                Location = new Point(i % 6 * 100 + 50 * (6-x), -x * 30 + 100+30*7),
                            });
                            Controls.Add(Bricks[Bricks.Count-1]);
                        }
                    }
                    break;
                case 3:
                    for (int x = 10; x >= 0; x--)
                    {
                        for (int i = 0; i < x; i++)
                        {
                            Bricks.Add(new PictureBox()
                            {
                                Size = new Size(58, 28),
                                BackColor = colors[x==10?3:x>6?2:x>3?1:0],
                                Name = (x == 10 ? 3 : x > 6 ? 2 : x > 3 ? 1 : 0).ToString(),
                                Location = new Point(i * 60, x * 30-30),
                            });
                            Controls.Add(Bricks[Bricks.Count - 1]);
                        }
                    }
                    break;
            }
        }
    }
}
