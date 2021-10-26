using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorMazeRound
{
    Vector2Round SizeMaze;//

    public MazeRound MazeGeneration(Vector2Round sizeMaze, Vector2Round startCell)//x - R (���������� ����� �� ������), y - L (���������� ����� � ������ �����)
    {
        SizeMaze = sizeMaze;

        List<MazeCellRound[]> cells = new List<MazeCellRound[]>();

        for (int r = 0; r < sizeMaze.r; r++)
        {
            float degree = Mathf.CeilToInt(Mathf.Log(r + 2, 2)) - 1;

            MazeCellRound[] cellsR = new MazeCellRound[(int)(Mathf.Pow(2, degree) * sizeMaze.l)];
            for (int l = 0; l < cellsR.Length; l++)
                cellsR[l] = new MazeCellRound { R = r, L = l };

            cells.Add(cellsR);
        }

        for (int r = 0; r < cells[0].Length; r++)
        {
            cells[0][r].WallTop = false;
        }

        for (int r = 0; r < cells[cells.Count - 1].Length; r++)
        {
            cells[cells.Count - 1][r].WallRigth = false;
            cells[cells.Count - 1][r].Floor = false;
        }
               
        PaveWay(cells, startCell);

        MazeRound maze = new MazeRound();

        maze.cells = cells;
        maze.startPosition = startCell;
        maze.finishPosition = PlaceMazeExit(cells, startCell);

        return maze;
    }

    private void PaveWay(List<MazeCellRound[]> maze, Vector2Round statrCell)
    {
        MazeCellRound current�ell = maze[statrCell.r][statrCell.l];

        current�ell.DistanceFromStart = 0;
        current�ell.Visited = true;

        Stack<MazeCellRound> stack = new Stack<MazeCellRound>();

        do
        {
            List<MazeCellRound> unvisitedCell = new List<MazeCellRound>();

            int R = current�ell.R;
            int L = current�ell.L;

            if (R < SizeMaze.r - 2)//������������ �� ������
            {
                if (Mathf.CeilToInt(Mathf.Log(R + 2, 2)) < Mathf.CeilToInt(Mathf.Log(R + 3, 2)))
                {
                    if (!maze[R + 1][L * 2].Visited) unvisitedCell.Add(maze[R + 1][L * 2]);
                    if (!maze[R + 1][L * 2 + 1].Visited) unvisitedCell.Add(maze[R + 1][L * 2 + 1]);
                }
                else
                {
                    if (!maze[R + 1][L].Visited) unvisitedCell.Add(maze[R + 1][L]);
                }
            }

            if (R > 0)//������������ � �����
            {
                if (Mathf.CeilToInt(Mathf.Log(R + 2, 2)) > Mathf.CeilToInt(Mathf.Log(R + 1, 2)))
                {
                    if (!maze[R - 1][Mathf.FloorToInt(L / 2f)].Visited) unvisitedCell.Add(maze[R - 1][Mathf.FloorToInt(L / 2f)]);
                }
                else
                {
                    if (!maze[R - 1][L].Visited) unvisitedCell.Add(maze[R - 1][L]);
                }
            }

            int sizeL = (int)(Mathf.Pow(2, (Mathf.CeilToInt(Mathf.Log(R + 2, 2)) - 1)) * SizeMaze.l);

            if (!maze[R][(L < sizeL - 1) ? (L + 1) : 0].Visited) unvisitedCell.Add(maze[R][(L < sizeL - 1) ? (L + 1) : 0]);
            if (!maze[R][(L > 0) ? (L - 1) : sizeL - 1].Visited) unvisitedCell.Add(maze[R][(L > 0) ? (L - 1) : sizeL - 1]);


            if (unvisitedCell.Count > 0)
            {
                MazeCellRound nextCell = unvisitedCell[Random.Range(0, unvisitedCell.Count)];
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

    private void RemoveWall(MazeCellRound a, MazeCellRound b)
    {
        if (a.R == b.R)
        {
            int sizeL = (int)(Mathf.Pow(2, (Mathf.CeilToInt(Mathf.Log(a.R + 2, 2)) - 1)) * SizeMaze.l);

            int nextL = a.L + 1;
            if (nextL >= sizeL) nextL -= sizeL;

            if (nextL == b.L) b.WallRigth = false;
            else a.WallRigth = false;
        }
        else
        {
            if (a.R > b.R) a.WallTop = false;
            else b.WallTop = false;
        }
    }

    private Vector2Round PlaceMazeExit(List<MazeCellRound[]> maze, Vector2Round statrCell)
    {
        MazeCellRound furthest = maze[statrCell.r][statrCell.l];

        for (int l = 0; l < maze[SizeMaze.r - 2].Length; l++)
        {
            if (maze[SizeMaze.r - 2][l].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[SizeMaze.r - 2][l];
        }

        List<MazeCellRound> finishCell = new List<MazeCellRound>();


        if (Mathf.CeilToInt(Mathf.Log(furthest.R + 2, 2)) < Mathf.CeilToInt(Mathf.Log(furthest.R + 3, 2)))
        {
            finishCell.Add(maze[furthest.R + 1][furthest.L * 2]);
            finishCell.Add(maze[furthest.R + 1][furthest.L * 2 + 1]);
        }
        else
        {
            finishCell.Add(maze[furthest.R + 1][furthest.L]);
        }

        MazeCellRound finish = finishCell[Random.Range(0, finishCell.Count)];
        finish.WallTop = false;

        return new Vector2Round(furthest.R, furthest.L);
    }
}

public class MazeRound
{
    public List<MazeCellRound[]> cells;

    public Vector2Round startPosition;
    public Vector2Round finishPosition;
}

public class MazeCellRound
{
    public int R;//��������� ������ �� ��� R
    public int L;//��������� ������ �� ��� L

    public bool WallRigth = true;//����� ����� 
    public bool WallTop = true;//������ ����� 
    public bool Floor = true;//���

    public bool Visited = false;//�������� �� � ��� ������?
    public int DistanceFromStart;//���������� ����� �� ������
}

[System.Serializable]
public struct Vector2Round
{
    public Vector2Round(int r, int l)
    {
        this.r = r;
        this.l = l;
    }

    public int r;//��������� ������ �� ��� R
    public int l;//��������� ������ �� ��� L
}