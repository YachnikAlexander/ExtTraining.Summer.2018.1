using System;
using System.Collections.Generic;

namespace MazeLibrary
{
    public class MazeSolver
    {

        public int[,] MazeModel { get; private set; }


        private List<List<Point>> routes = new List<List<Point>>();
        //adding the borders for finding end  of route
        private int[,] updateArray;
        private int startX;
        private int startY;


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
            this.startX = startX;
            this.startY = startY;
        }

        public int[,] MazeWithPass()
        {
            int length = routes[0].Count;
            List<Point> shortestRoute = routes[0];

            for (int i = 1; i < routes.Count; i++)
            {
                if (routes[i].Count < length)
                {
                    length = routes[i].Count;
                    shortestRoute = routes[i];
                }
            }

            for (int i = 0; i < shortestRoute.Count; i++)
            {
                MazeModel[shortestRoute[i].X - 1, shortestRoute[i].Y - 1] = i + 1;
            }
            return MazeModel;
        }

        public void PassMaze()
        {
            int length = (int)Math.Sqrt(MazeModel.Length);
            updateArray = new int[length + 2, length + 2];
            for (int i = 0; i < length + 2; i++)
            {
                for (int j = 0; j < length + 2; j++)
                {
                    if (i == 0
                        || j == 0
                        || i == length + 1
                        || j == length + 1)
                    {
                        updateArray[i, j] = -2;
                    }
                    else
                    {
                        updateArray[i, j] = MazeModel[i - 1, j - 1];
                    }
                }
            }

            updateArray[startX + 1, startY + 2] = -10;
            Point point = new Point(startX + 1, startY + 1, 0);
            List<Point> route = new List<Point>();
            route.Add(point);
            routes.Add(route);

            StartFindRoute(point);

        }

        private void StartFindRoute(Point point)
        {
            List<Point> neighbors = new List<Point>();

            for (int i = point.X - 1; i <= point.X + 1; i++)
            {
                for (int j = 0; j <= point.Y + 1; j++)
                {
                    Point newPoint = new Point(i, j, updateArray[i, j]);
                    double length = CalculateLentghBetweenPoints(point, newPoint);

                    if (length == 1 && newPoint.Number == 0)
                    {

                        if (CheckListOfPoint(point) != -1)
                        {

                            neighbors.Add(newPoint);
                        }
                    }

                    if (newPoint.Number == -2 && length == 1)
                    {
                        return; //find the end of labirint
                    }
                }
            }
            SplitLists(point, neighbors);
        }

        private void SplitLists(Point point, List<Point> neighbors)
        {
            int containIndexOfList = CheckListOfPoint(point);
            for (int i = 0; i < neighbors.Count; i++)
            {
                int index = CheckListOfPoint(neighbors[i]);
                if (index != -1)
                {
                    neighbors.RemoveAt(i);
                }
            }

            if(neighbors.Count == 0)
            {
                routes.RemoveAt(containIndexOfList);
                return;
            }

            if (neighbors.Count == 1)
            {
                routes[containIndexOfList].Add(neighbors[0]);
                StartFindRoute(neighbors[0]);
            }
            else
            {
                for(int i = 0; i < neighbors.Count; i++)
                {
                    List<Point> copyRoute = new List<Point>(routes[containIndexOfList]);
                    copyRoute.Add(neighbors[i]);
                    routes.Add(copyRoute);
                }
                routes.RemoveAt(containIndexOfList);
                for (int i = 0; i < neighbors.Count; i++)
                {
                    StartFindRoute(neighbors[i]);
                }
            }
        }

        private int CheckListOfPoint(Point point)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                for (int j = 0; j < routes[i].Count; j++)
                {
                    if (point.X == routes[i][j].X && point.Y == routes[i][j].Y)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private double CalculateLentghBetweenPoints(Point lhs, Point rhs)
        {
            int x = lhs.X - rhs.X;
            int y = lhs.Y - rhs.Y;

            return Math.Sqrt(x * x + y * y);
        }

        public class Point
        {
            public bool Contains { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Number { get; set; }

            public Point(int x, int y, int number)
            {
                this.X = x;
                this.Y = y;
                this.Contains = false;
                this.Number = number;
            }
        }
    }
}
