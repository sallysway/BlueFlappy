using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;
using Flappy.Properties;

namespace Flappy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<int> Tube = new List<int>();
        List<int> Tube2 = new List<int>();

        int TubeWidth = 55;
        int TubeDifferentY = 140;
        int TubeDifferentX = 180;
        bool start = true;
        bool running;
        int speed = 3;
        int OriginalX, OriginalY;
        bool ResetTubes= false;
        int points;
        bool inTube = false;
        int score;
        int ScoreDifferent;

        private void Die()
        {
            running = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;
            ReadAndShowScore();
            points = 0;
            pictureBox1.Location = new Point(OriginalX, OriginalY);
            pictureBox1.Image = Flappy.Properties.Resources.deadBird;
            ResetTubes= true;
            Tube.Clear();

        }

        private void ReadAndShowScore()
        {
            using (StreamReader reader = new StreamReader("Score.ini"))
            {
                score = int.Parse(reader.ReadToEnd());
                reader.Close();
                if (int.Parse(label1.Text) == 0 | int.Parse(label1.Text) > 0)
                {
                    ScoreDifferent = score - int.Parse(label1.Text) + 1;
                }
                if (score < int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Your score was bigger than {0}. New score: {1}", score, label1.Text), "BlueBird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    using (StreamWriter writer = new StreamWriter("Score.ini"))
                    {
                        writer.Write(label1.Text);
                        writer.Close();
                    }
                }

                if (score > int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("You needed {0} to win. Biggest score was  {1}", ScoreDifferent, score), "BlueBird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (score == int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("The score is {0} (maximum score). Try to beat it!", "BlueBird", MessageBoxButtons.OK, MessageBoxIcon.Information));
                }
            }
        }

        private void StartGame()
        {
            ResetTubes = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            Random random = new Random();
            int num = random.Next(40, (this.Height - this.TubeDifferentY));
            int num1 = num + this.TubeDifferentY;
            Tube.Clear();
            Tube.Add(this.Width);
            Tube.Add(num);
            Tube.Add(this.Width);
            Tube.Add(num1);

            num = random.Next(40, (this.Height - TubeDifferentY));
            num1 = num + this.TubeDifferentY;
            Tube2.Clear();
            Tube2.Add(this.Width + TubeDifferentX);
            Tube2.Add(num);
            Tube2.Add(this.Width + TubeDifferentX);
            Tube2.Add(num1);

            button1.Visible = false;
            button1.Enabled = false;
            running = true;
            Focus();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Tube[0] + TubeWidth <= 0 | start == true)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(this.Height - TubeDifferentY);
                var p2x = px;
                var p2y = py + TubeDifferentY;
                Tube.Clear();
                Tube.Add(px);
                Tube.Add(py);
                Tube.Add(p2x);
                Tube.Add(p2y);

            }
            else
            {
                Tube[0] = Tube[0] - 2;
                Tube[2] = Tube[2] - 2;
            }
            if (Tube2[0] + TubeWidth <= 0)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(this.Height - TubeDifferentY);
                var p2x = px;
                var p2y = py + TubeDifferentY;
                int[] p1 = { px, py, p2x, p2y };
                Tube2.Clear();
                Tube2.Add(px);
                Tube2.Add(py);
                Tube2.Add(p2x);
                Tube2.Add(p2y);

            }
            else
            {
                Tube2[0] = Tube2[0] - 2;
                Tube2[2] = Tube2[2] - 2;
            }
            if (start == true)
            {
                start = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!ResetTubes && Tube.Any() && Tube2.Any())
            {
                //first on top
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube[0], 0, TubeWidth, Tube[1]));
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube[0] - 10, Tube[3] - TubeDifferentY, 75, 15));

                //first bottom
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube[2], Tube[3], TubeWidth, this.Height - Tube[3]));
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube[2] - 10, Tube[3], 75, 15));

                //second above
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube2[0], 0, TubeWidth, Tube2[1]));
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube2[0] - 10, Tube2[3] - TubeDifferentY, 75, 15));

                //second bottom
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube2[2], Tube2[3], TubeWidth, this.Height - Tube2[3]));
                e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(Tube2[2] - 10, Tube2[3], 75, 15));
            }
        }


        private void CheckForPoint()
        {
            Rectangle bird = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Tube[2] + 20, Tube[3] - TubeDifferentY, 15, TubeDifferentY);
            Rectangle rec2 = new Rectangle(Tube2[2] + 20, Tube2[3] - TubeDifferentY, 15, TubeDifferentY);
            Rectangle interesect1 = Rectangle.Intersect(bird, rec1);
            Rectangle interesect2 = Rectangle.Intersect(bird, rec2);

            if (!ResetTubes | start)
            {
                if (interesect1 != Rectangle.Empty | interesect2 != Rectangle.Empty)
                {
                    if (!inTube)
                    {
                        points++;
                        SoundPlayer s = new SoundPlayer(Flappy.Properties.Resources.Boing);
                        s.Play();
                        inTube = true;
                    }
                }
                else
                {
                    SoundPlayer s = new SoundPlayer(Flappy.Properties.Resources.win);
                    s.Play();
                    inTube = false;
                }
            }

        }

        private void CheckForCollision()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Tube[0], 0, TubeWidth, Tube[1]);
            Rectangle rec2 = new Rectangle(Tube[2], Tube[3], TubeWidth, this.Height - Tube[3]);
            Rectangle rec3 = new Rectangle(Tube2[0], 0, TubeWidth, Tube2[1]);
            Rectangle rec4 = new Rectangle(Tube2[2], Tube2[3], TubeWidth, this.Height - Tube2[3]);

            Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersect3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersect4 = Rectangle.Intersect(rec, rec4);

            //if we are playing and we have collided with one of the pipes
            if (!ResetTubes | start)
            {
                if (intersect1 != Rectangle.Empty | intersect2 != Rectangle.Empty | intersect3 != Rectangle.Empty | intersect4 != Rectangle.Empty)

                {
                    SoundPlayer p = new SoundPlayer(Flappy.Properties.Resources.Boing);
                    p.Play();
                    Die();
                }

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    speed = -5;
                    pictureBox1.Image = Resources.flightBird;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    speed = 5;
                    pictureBox1.Image = Flappy.Properties.Resources.flightBird;
                    break;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + speed);
            //make sure bird doesnt exit the window
            if (pictureBox1.Location.Y < 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }
            if (pictureBox1.Location.Y + pictureBox1.Height > this.ClientSize.Height)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, this.ClientSize.Height - pictureBox1.Height);
            }
            CheckForCollision();

            if (running)
            {
                CheckForPoint();
            }
            label1.Text = Convert.ToString(points);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OriginalX = pictureBox1.Location.X;
            OriginalY = pictureBox1.Location.Y;

            if (!File.Exists("Score.ini"))
            {
                File.Create("Score.ini").Dispose();
            }
        }
    }
}
