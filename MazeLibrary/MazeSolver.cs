using System;
using System.Collections.Generic;

namespace MazeLibrary
{
    public class MazeSolver
    {

        public int[,] MazeModel { get; private set; }
        public int StartX { get; private set; }
        public int StartY { get; private set; }

        List<Point> badPoints = new List<Point>();
        public List<Point> Route { get; private set; }

        public MazeSolver(int[,] mazeModel, int startX, int startY)
        {

            if (mazeModel == null)
            {
                throw new ArgumentNullException($"{0} is null ", nameof(mazeModel));
            }

            if (startX < 0)
            {
                throw new ArgumentException($"{0} less than 0", nameof(startX));
            }

            if (startY < 0)
            {
                throw new ArgumentException($"{0} is less than 0", nameof(startY));
            }

            this.MazeModel = mazeModel;
            this.StartX = startX;
            this.StartY = startY;

            ///throw new NotImplementedException();
        }

        public int[,] MazeWithPass()
        {
            int lengthX = this.MazeModel.GetUpperBound(0) + 1;
            int lengthY = this.MazeModel.GetUpperBound(1) + 1;

            for (int i = 0; i < lengthX; i++)
            {
                for (int j = 0; j < lengthY; j++)
                {
                    if (Route.Contains(new Point(i, j)))
                    {
                        int step = Route.IndexOf(new Point(i, j)) + 1;
                        MazeModel[i, j] = step;
                    }
                }
            }
            return MazeModel;
        }

        public void PassMaze()
        {
            int lengthX = this.MazeModel.GetUpperBound(0) + 1;
            int lengthY = this.MazeModel.GetUpperBound(1) + 1;


            Point point = new Point(this.StartX, this.StartY);
            Route = new List<Point>();
            Route.Add(point);

            for (int i = point.X; i < lengthX;)
            {
                for (int j = point.Y; j < lengthY;)
                {
                    Point newPoint = GetNewPoint(point);
                    Point lastPoint = new Point((Route[Route.Count - 1].X), (Route[Route.Count - 1].Y));

                    if (newPoint == null)
                    {
                        for (int l = 0; l < badPoints.Count; l++)
                        {
                            Route.Add(badPoints[l]);
                        }
                        break;
                    }

                    if (!CheckBadPoint(newPoint))
                    {
                        Route.Add(newPoint);
                        point = newPoint;
                    }
                    else
                    {
                        int lenRoute = Route.Count - 1;
                        for (int k = lenRoute; k > 0; k--)
                        {
                            if (Route[k].Flag == false)
                            {
                                badPoints.Add(newPoint);
                                newPoint = Route[Route.Count - 1];
                                Route.RemoveAt(Route.Count - 1);
                            }
                            else
                            {
                                point = newPoint;
                                break;
                            }
                        }

                    }


                }
            }
        }

        private bool CheckBadPoint(Point point)
        {
            for (int i = 0; i < badPoints.Count; i++)
            {

                if (point.X == badPoints[i].X && point.Y == badPoints[i].Y)
                {
                    return true;
                }
            }

            return false;
        }

        private Point GetNewPoint(Point point)
        {
            int lengthX = this.MazeModel.GetUpperBound(0);
            int lengthY = this.MazeModel.GetUpperBound(1);

            List<Point> neighbors = new List<Point>();
            if (point.X + 1 < lengthX)
            {
                Point newPoint = new Point(point.X + 1, point.Y);
                int firstNumber = MazeModel[point.X, point.Y];
                int secondNumber = MazeModel[newPoint.X, newPoint.Y];

                if (Compare(firstNumber, secondNumber) == 0)
                {
                    if (!CheckBadPoint(newPoint))
                    {
                        neighbors.Add(newPoint);
                    }
                }
            }

            if (point.Y + 1 < lengthY)
            {
                Point newPoint = new Point(point.X, point.Y + 1);
                int firstNumber = MazeModel[point.X, point.Y];
                int secondNumber = MazeModel[newPoint.X, newPoint.Y];

                if (Compare(firstNumber, secondNumber) == 0)
                {
                    if (!CheckBadPoint(newPoint))
                    {
                        neighbors.Add(newPoint);
                    }
                }
            }

            if (point.Y - 1 < lengthY)
            {
                Point newPoint = new Point(point.X, point.Y - 1);
                int firstNumber = MazeModel[point.X, point.Y];

                int secondNumber = MazeModel[newPoint.X, newPoint.Y];

                if (Compare(firstNumber, secondNumber) == 0)
                {
                    if (!CheckBadPoint(newPoint))
                    {
                        neighbors.Add(newPoint);
                    }
                }
            }

            if (point.X - 1 < lengthX)
            {
                Point newPoint = new Point(point.X - 1, point.Y);
                int firstNumber = MazeModel[point.X, point.Y];
                int secondNumber = MazeModel[newPoint.X, newPoint.Y];

                if (Compare(firstNumber, secondNumber) == 0)
                {
                    if (!CheckBadPoint(newPoint))
                    {
                        neighbors.Add(newPoint);
                    }
                }
            }

            Point neighbor = null;
            if (neighbors.Count == 0)
            {
                neighbor = null;
            }

            if (neighbors.Count >= 1)
            {
                neighbor = neighbors[0];
                point.Flag = false;
            }

            return neighbor;
        }

        public class Point
        {
            public bool Flag { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
                this.Flag = false;
            }
        }



        private int Compare(int first, int second)
        {
            return first - second;
        }
    }
}
