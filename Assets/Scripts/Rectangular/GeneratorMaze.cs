using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorMaze
{
    Vector2Int SizeMaze;

    public Maze MazeGeneration(Vector2Int sizeMaze, Vector2Int startCell)
    {
        SizeMaze = sizeMaze;

        MazeCell[,] cells = new MazeCell[sizeMaze.x, sizeMaze.y];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeCell { X = x, Y = y };
            }
        }

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            cells[x, sizeMaze.y - 1].WallLeft = false;
            cells[x, sizeMaze.y - 1].Floor = false;
        }

        for (int y = 0; y < cells.GetLength(1); y++)
        {
            cells[sizeMaze.x - 1, y].WallBottom = false;
            cells[sizeMaze.x - 1, y].Floor = false;
        }

        PaveWay(cells, startCell);

        Maze maze = new Maze();

        maze.cells = cells;
        maze.startPosition = startCell;
        maze.finishPosition = PlaceMazeExit(cells, startCell);

        return maze;
    }

    private void PaveWay(MazeCell[,] maze, Vector2Int statrCell)
    {
        MazeCell current�ell = maze[statrCell.x, statrCell.y];

        current�ell.DistanceFromStart = 0;
        current�ell.Visited = true;

        Stack<MazeCell> stack = new Stack<MazeCell>();

        do
        {
            List<MazeCell> unvisitedCell = new List<MazeCell>();

            int x = current�ell.X;
            int y = current�ell.Y;

            /* //��������� � ����������� ������ � ������:
            if (x > 0 && (!maze[x - 1, y].Visited || Random.Range(0, 200) < 1)) unvisitedCell.Add(maze[x - 1, y]);
            if (y > 0 && (!maze[x, y - 1].Visited || Random.Range(0, 200) < 1)) unvisitedCell.Add(maze[x, y - 1]);
            if (x < SizeMaze.x - 2 && (!maze[x + 1, y].Visited || Random.Range(0, 200) < 1)) unvisitedCell.Add(maze[x + 1, y]);
            if (y < SizeMaze.y - 2 && (!maze[x, y + 1].Visited || Random.Range(0, 200) < 1)) unvisitedCell.Add(maze[x, y + 1]);
            */

            if (x > 0 && !maze[x - 1, y].Visited) unvisitedCell.Add(maze[x - 1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedCell.Add(maze[x, y - 1]);
            if (x < SizeMaze.x - 2 && !maze[x + 1, y].Visited) unvisitedCell.Add(maze[x + 1, y]);
            if (y < SizeMaze.y - 2 && !maze[x, y + 1].Visited) unvisitedCell.Add(maze[x, y + 1]);

            if (unvisitedCell.Count > 0)
            {
                MazeCell nextCell = unvisitedCell[Random.Range(0, unvisitedCell.Count)];
                RemoveWall(current�ell, nextCell);

                stack.Push(nextCell);
                nextCell.DistanceFromStart = current�ell.DistanceFromStart + 1;
                current�ell = nextCell;
                current�ell.Visited = true;
            }
            else
            {
                current�ell = stack.Pop();
            }
        }
        while (stack.Count > 0);
    }

    private void RemoveWall(MazeCell a, MazeCell b)
    {
        if (a.X == b.X)
        {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else
        {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }

    private Vector2Int PlaceMazeExit(MazeCell[,] maze, Vector2Int statrCell)
    {
        MazeCell furthest = maze[statrCell.x, statrCell.y];

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, SizeMaze.y - 2].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, SizeMaze.y - 2];
            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, 0];
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            if (maze[SizeMaze.x - 2, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[SizeMaze.x - 2, y];
            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[0, y];
        }

        if (furthest.X == 0) furthest.WallLeft = false;
        else if (furthest.Y == 0) furthest.WallBottom = false;
        else if (furthest.X == SizeMaze.x - 2) maze[furthest.X + 1, furthest.Y].WallLeft = false;
        else if (furthest.Y == SizeMaze.y - 2) maze[furthest.X, furthest.Y + 1].WallBottom = false;

        return new Vector2Int(furthest.X, furthest.Y);
    }
}

public class Maze
{
    public MazeCell[,] cells;

    public Vector2Int startPosition;
    public Vector2Int finishPosition;
}

public class MazeCell
{
    public int X;//��������� ������ �� ��� X
    public int Y;//��������� ������ �� ��� Y

    public bool WallLeft = true;//����� ����� 
    public bool WallBottom = true;//������ ����� 
    public bool Floor = true;//���

    public bool Visited = false;//�������� �� � ��� ������?
    public int DistanceFromStart;//���������� ����� �� ������
}