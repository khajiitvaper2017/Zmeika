using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Zmeika
{
    public class Snake
    {
        public enum Direction
        {
            Forward,
            Down,
            Left,
            Right
        }

        private readonly Random _random = new Random();
        private readonly List<Point> _snake = new List<Point>();
        private int _foodCount;
        private Point _head;
        private Direction _lastDirection;

        public bool State;
        public byte[,] Field { get; set; }
        public Direction CurrentDirection { get; set; }

        public void Process()
        {
            for (var x = 0; x < Field.GetLength(0); x++)
            for (var y = 0; y < Field.GetLength(1); y++)
                if (Field[x, y] == 1 && !_snake.Exists(s => s.X == x && s.Y == y))
                    _snake.Add(new Point(x, y));

            for (var x = 0; x < Field.GetLength(0); x++)
            for (var y = 0; y < Field.GetLength(1); y++)
                if (Field[x, y] == 3)
                    _snake.Add(new Point(x, y));

            _head = _snake.Last();

            X:
            switch (CurrentDirection)
            {
                case Direction.Forward:
                    if (_lastDirection != Direction.Down)
                    {
                        _snake.Add(new Point(_head.X, _head.Y - 1));
                        _lastDirection = Direction.Forward;
                    }
                    else
                    {
                        CurrentDirection = _lastDirection;
                        goto X;
                    }

                    break;
                case Direction.Down:
                    if (_lastDirection != Direction.Forward)
                    {
                        _snake.Add(new Point(_head.X, _head.Y + 1));
                        _lastDirection = Direction.Down;
                    }
                    else
                    {
                        CurrentDirection = _lastDirection;
                        goto X;
                    }

                    break;
                case Direction.Left:
                    if (_lastDirection != Direction.Right)
                    {
                        _snake.Add(new Point(_head.X - 1, _head.Y));
                        _lastDirection = Direction.Left;
                    }
                    else
                    {
                        CurrentDirection = _lastDirection;
                        goto X;
                    }

                    break;
                case Direction.Right:
                    if (_lastDirection != Direction.Left)
                    {
                        _snake.Add(new Point(_head.X + 1, _head.Y));
                        _lastDirection = Direction.Right;
                    }
                    else
                    {
                        CurrentDirection = _lastDirection;
                        goto X;
                    }

                    break;
            }

            _head = _snake.Last();
            State = true;
            if (Field[(_head.X + Field.GetLength(0)) % Field.GetLength(0),
                (_head.Y + Field.GetLength(1)) % Field.GetLength(1)] == 1)
                State = false;
            else if (Field[(_head.X + Field.GetLength(0)) % Field.GetLength(0),
                (_head.Y + Field.GetLength(1)) % Field.GetLength(1)] == 3)
                State = false;

            if (Field[(_head.X + Field.GetLength(0)) % Field.GetLength(0),
                (_head.Y + Field.GetLength(1)) % Field.GetLength(1)] != 2)
            {
                _snake.Remove(_snake.First());
                _snake.Remove(_snake.First());
            }
            else
            {
                _foodCount--;
            }

            for (var x = 0; x < Field.GetLength(0); x++)
            for (var y = 0; y < Field.GetLength(1); y++)
                if (Field[x, y] == 1 || Field[x, y] == 3)
                    Field[x, y] = 0;
            foreach (var point in _snake)
                Field[(point.X + Field.GetLength(0)) % Field.GetLength(0),
                    (point.Y + Field.GetLength(1)) % Field.GetLength(1)] = 1;

            Field[(_head.X + Field.GetLength(0)) % Field.GetLength(0),
                (_head.Y + Field.GetLength(1)) % Field.GetLength(1)] = 3;
        }

        public void FoodCountAndCreate()
        {
            if (_foodCount != 0) return;
            Field[_random.Next(Field.GetLength(0)), _random.Next(Field.GetLength(1))] = 2;
            _foodCount++;
        }
    }
}