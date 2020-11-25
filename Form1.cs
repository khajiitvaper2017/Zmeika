using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zmeika
{
    public partial class Form1 : Form
    {
        private bool GameGoing = false;

        private enum Direction
        {
            Forward,
            Down,
            Left,
            Right
        }

        private Direction direction = Direction.Right;
        private Direction lastdirection;

        private Random random = new Random();
        private Graphics graphics;
        private int scale = 20;
        private byte[,] field;
        private List<Snake> snake = new List<Snake>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (GameGoing)
            {
                graphics.Clear(DefaultBackColor);
                ProcessGame();
                Render();
                pictureBox1.Refresh();
            }
        }

        private void ProcessGame()
        {
            int foodCount = 0;
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y] == 2)
                    {
                        foodCount += 1;
                    }
                }
            }
            if (foodCount == 0)
            {
                field[random.Next(1, field.GetLength(0) - 1), random.Next(1, field.GetLength(1) - 1)] = 2;
            }

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y] == 1)
                    {
                        var newsnake = new Snake(x, y);

                        if (!snake.Exists(s => s.X == newsnake.X && s.Y == newsnake.Y))
                        {
                            snake.Add(newsnake);
                        }
                    }
                }
            }
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y] == 3)
                    {
                        snake.Add(new Snake(x, y));
                    }
                }
            }
            Snake head = snake.Last();
        X:;
            switch (direction)
            {
                case Direction.Forward:
                    if (lastdirection != Direction.Down)
                    {
                        snake.Add(new Snake(head.X, head.Y - 1));
                        lastdirection = Direction.Forward;
                    }
                    else
                    {
                        direction = lastdirection;
                        goto X;
                    }
                    break;

                case Direction.Down:
                    if (lastdirection != Direction.Forward)
                    {
                        snake.Add(new Snake(head.X, head.Y + 1));
                        lastdirection = Direction.Down;
                    }
                    else
                    {
                        direction = lastdirection;
                        goto X;
                    }
                    break;

                case Direction.Left:
                    if (lastdirection != Direction.Right)
                    {
                        snake.Add(new Snake(head.X - 1, head.Y));
                        lastdirection = Direction.Left;
                    }
                    else
                    {
                        direction = lastdirection;
                        goto X;
                    }
                    break;

                case Direction.Right:
                    if (lastdirection != Direction.Left)
                    {
                        snake.Add(new Snake(head.X + 1, head.Y));
                        lastdirection = Direction.Right;
                    }
                    else
                    {
                        direction = lastdirection;
                        goto X;
                    }
                    break;

                default:
                    break;
            }
            head = snake.Last();
            if (field[(head.X + field.GetLength(0)) % field.GetLength(0), (head.Y + field.GetLength(1)) % field.GetLength(1)] == 1 ||
                field[(head.X + field.GetLength(0)) % field.GetLength(0), (head.Y + field.GetLength(1)) % field.GetLength(1)] == 3)
            {
                GameGoing = false;
                button1.Enabled = true;
                button1.Visible = true;
                button1.Text = "You Loose! \nClick To Start Again!";
            }
            if (field[(head.X + field.GetLength(0)) % field.GetLength(0), (head.Y + field.GetLength(1)) % field.GetLength(1)] != 2)
            {
                snake.Remove(snake.First());
                snake.Remove(snake.First());
            }

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y] == 1 || field[x, y] == 3)
                    {
                        field[x, y] = 0;
                    }
                }
            }
            foreach (var snak in snake)
            {
                field[(snak.X + field.GetLength(0)) % field.GetLength(0), (snak.Y + field.GetLength(1)) % field.GetLength(1)] = 1;
            }

            field[(head.X + field.GetLength(0)) % field.GetLength(0), (head.Y + field.GetLength(1)) % field.GetLength(1)] = 3;
        }

        private void Render()
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y] != 0)
                    {
                        if (field[x, y] == 1)
                        {
                            graphics.FillRectangle(Brushes.Green, x * scale, y * scale, scale - 1, scale - 1);
                        }
                        else if (field[x, y] == 3)
                        {
                            graphics.FillRectangle(Brushes.DarkGreen, x * scale, y * scale, scale - 1, scale - 1);
                        }
                        else if (true)
                        {
                            graphics.FillRectangle(Brushes.Red, x * scale, y * scale, scale - 1, scale - 1);
                        }
                    }
                    else
                    {
                        graphics.FillRectangle(Brushes.Black, x * scale, y * scale, scale - 1, scale - 1);
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    direction = Direction.Forward;
                    break;

                case Keys.Up:
                    direction = Direction.Forward;
                    break;

                case Keys.A:
                    direction = Direction.Left;
                    break;

                case Keys.Left:
                    direction = Direction.Left;
                    break;

                case Keys.S:
                    direction = Direction.Down;
                    break;

                case Keys.Down:
                    direction = Direction.Down;
                    break;

                case Keys.D:
                    direction = Direction.Right;
                    break;

                case Keys.Right:
                    direction = Direction.Right;
                    break;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            if (GameGoing)
            {
                random = new Random();
                snake = new List<Snake>();

                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                graphics = Graphics.FromImage(pictureBox1.Image);
                field = new byte[pictureBox1.Width / scale, pictureBox1.Height / scale];

                field[pictureBox1.Width / scale / 2, pictureBox1.Height / scale / 2] = 3;
                //field[(pictureBox1.Width / scale / 2) - 1, pictureBox1.Height / scale / 2] = 1;

                timer1.Start();
            }
        }

        public class PictureBoxWithInterpolationMode : PictureBox
        {
            protected override void OnPaint(PaintEventArgs paintEventArgs)
            {
                paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                base.OnPaint(paintEventArgs);
            }
        }

        public class Snake
        {
            public int X { get; set; }

            public int Y { get; set; }

            public Snake(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GameGoing = true;
            button1.Enabled = false;
            button1.Visible = false;
            StartGame(sender, e);
        }

        private void Button1_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}