using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

// ReSharper disable IdentifierTypo

// ReSharper disable InconsistentNaming

namespace Zmeika
{
    public partial class Form1 : Form
    {
        private const int scale = 20;
        private Direction direction = Direction.Right;
        private byte[,] field;
        private bool GameGoing;
        private Graphics graphics;
        private Direction lastDirection;

        private Random random = new Random();
        private List<Point> snake = new List<Point>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (!GameGoing) return;
            graphics.Clear(DefaultBackColor);
            FoodCountAndCreate();
            Snake();
            Render();
            pictureBox1.Refresh();
        }

        private void Snake()
        {
            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] == 1 && !snake.Exists(s => s.X == x && s.Y == y))
                    snake.Add(new Point(x, y));


            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] == 3)
                    snake.Add(new Point(x, y));
            var head = snake.Last();
            X:

            switch (direction)
            {
                case Direction.Forward:
                    if (lastDirection != Direction.Down)
                    {
                        snake.Add(new Point(head.X, head.Y - 1));
                        lastDirection = Direction.Forward;
                    }
                    else
                    {
                        direction = lastDirection;
                        goto X;
                    }

                    break;

                case Direction.Down:
                    if (lastDirection != Direction.Forward)
                    {
                        snake.Add(new Point(head.X, head.Y + 1));
                        lastDirection = Direction.Down;
                    }
                    else
                    {
                        direction = lastDirection;
                        goto X;
                    }

                    break;

                case Direction.Left:
                    if (lastDirection != Direction.Right)
                    {
                        snake.Add(new Point(head.X - 1, head.Y));
                        lastDirection = Direction.Left;
                    }
                    else
                    {
                        direction = lastDirection;
                        goto X;
                    }

                    break;

                case Direction.Right:
                    if (lastDirection != Direction.Left)
                    {
                        snake.Add(new Point(head.X + 1, head.Y));
                        lastDirection = Direction.Right;
                    }
                    else
                    {
                        direction = lastDirection;
                        goto X;
                    }

                    break;
            }

            head = snake.Last();
            if (field[(head.X + field.GetLength(0)) % field.GetLength(0),
                    (head.Y + field.GetLength(1)) % field.GetLength(1)] == 1 ||
                field[(head.X + field.GetLength(0)) % field.GetLength(0),
                    (head.Y + field.GetLength(1)) % field.GetLength(1)] == 3)
            {
                GameGoing = false;
                button1.Enabled = true;
                button1.Visible = true;
                button1.Text = "You Loose! \n" +
                               "Click To Start Again!";
            }

            if (field[(head.X + field.GetLength(0)) % field.GetLength(0),
                (head.Y + field.GetLength(1)) % field.GetLength(1)] != 2)
            {
                snake.Remove(snake.First());
                snake.Remove(snake.First());
            }

            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] == 1 || field[x, y] == 3)
                    field[x, y] = 0;
            foreach (var point in snake)
                field[(point.X + field.GetLength(0)) % field.GetLength(0),
                    (point.Y + field.GetLength(1)) % field.GetLength(1)] = 1;

            field[(head.X + field.GetLength(0)) % field.GetLength(0),
                (head.Y + field.GetLength(1)) % field.GetLength(1)] = 3;
        }

        private void FoodCountAndCreate()
        {
            var foodCount = 0;
            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] == 2)
                    foodCount += 1;
            if (foodCount == 0)
                field[random.Next(1, field.GetLength(0) - 1), random.Next(1, field.GetLength(1) - 1)] = 2;
        }

        private void Render()
        {
            for (var x = 0; x < field.GetLength(0); x++)
            for (var y = 0; y < field.GetLength(1); y++)
                if (field[x, y] != 0)
                {
                    if (field[x, y] == 1)
                        graphics.FillRectangle(Brushes.Green, x * scale, y * scale, scale - 1, scale - 1);
                    else if (field[x, y] == 3)
                        graphics.FillRectangle(Brushes.DarkGreen, x * scale, y * scale, scale - 1, scale - 1);
                    else if (true) graphics.FillRectangle(Brushes.Red, x * scale, y * scale, scale - 1, scale - 1);
                }
                else
                {
                    graphics.FillRectangle(Brushes.Black, x * scale, y * scale, scale - 1, scale - 1);
                }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = Direction.Forward;
                    break;

                case Keys.Left:
                    direction = Direction.Left;
                    break;

                case Keys.Down:
                    direction = Direction.Down;
                    break;

                case Keys.Right:
                    direction = Direction.Right;
                    break;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            if (!GameGoing) return;
            random = new Random();
            snake = new List<Point>();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            field = new byte[pictureBox1.Width / scale, pictureBox1.Height / scale];

            field[pictureBox1.Width / scale / 2, pictureBox1.Height / scale / 2] = 3;

            timer1.Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GameGoing = true;
            button1.Enabled = false;
            button1.Visible = false;
            StartGame(sender, e);
        }


        private enum Direction
        {
            Forward,
            Down,
            Left,
            Right
        }

        public class PictureBoxWithInterpolationMode : PictureBox
        {
            protected override void OnPaint(PaintEventArgs paintEventArgs)
            {
                paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                base.OnPaint(paintEventArgs);
            }
        }
    }
}