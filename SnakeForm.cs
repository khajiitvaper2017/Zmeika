using System;
using System.Drawing;
using System.Windows.Forms;

// ReSharper disable InconsistentNaming

namespace Zmeika
{
    public partial class SnakeForm : Form
    {
        private const int Scaling = 20;
        private Snake.Direction direction = Snake.Direction.Right;
        private byte[,] field;
        private bool GameGoing;
        private Graphics graphics;
        private Snake Snake;

        public SnakeForm()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (!GameGoing) return;
            graphics.Clear(DefaultBackColor);

            Snake.Field = field;
            Snake.CurrentDirection = direction;
            Snake.FoodCountAndCreate();
            Snake.Process();
            GameGoing = Snake.State;

            Render();
            pictureBox1.Refresh();

            if (GameGoing) return;
            button1.Enabled = true;
            button1.Visible = true;
            button1.Text = "You Loose! \n" +
                           "Click To Start Again!";
        }

        private void Render()
        {
            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] != 0)
                {
                    if (field[x, y] == 1)
                        graphics.FillRectangle(Brushes.Green, x * Scaling, y * Scaling, Scaling - 1, Scaling - 1);
                    else if (field[x, y] == 3)
                        graphics.FillRectangle(Brushes.DarkGreen, x * Scaling, y * Scaling, Scaling - 1, Scaling - 1);
                    else if (true)
                        graphics.FillRectangle(Brushes.Red, x * Scaling, y * Scaling, Scaling - 1, Scaling - 1);
                }
                else
                {
                    graphics.FillRectangle(Brushes.Black, x * Scaling, y * Scaling, Scaling - 1, Scaling - 1);
                }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = Snake.Direction.Forward;
                    break;

                case Keys.Left:
                    direction = Snake.Direction.Left;
                    break;

                case Keys.Down:
                    direction = Snake.Direction.Down;
                    break;

                case Keys.Right:
                    direction = Snake.Direction.Right;
                    break;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            if (!GameGoing) return;


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            field = new byte[pictureBox1.Width / Scaling, pictureBox1.Height / Scaling];

            field[pictureBox1.Width / Scaling / 2, pictureBox1.Height / Scaling / 2] = 3;

            Snake = new Snake();
            timer1.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GameGoing = true;
            button1.Enabled = false;
            button1.Visible = false;
            StartGame(sender, e);
        }
    }
}