using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingRound : MonoBehaviour
{
    public LineRenderer componentLineRenderer;
    void Awake()
    {
        componentLineRenderer = GetComponent<LineRenderer>();
    }

    Vector2Int calculatoinPos(MazeRound maze, Vector2 finishPosition)
    {
        float Rnew = Vector2.Distance(finishPosition, new Vector2(0, 0));
        Rnew = Mathf.Min(Rnew, maze.cells.Count - 1);
        float sectorsNew = 360f / maze.cells[(int)Rnew].Length;

        float angle;
        float angleC = Mathf.Acos(finishPosition.x / Rnew) * Mathf.Rad2Deg;
        float angleS = Mathf.Asin(finishPosition.y / Rnew) * Mathf.Rad2Deg;
        if (angleS < 0) { angle = 360 - angleC; }
        else angle = angleC;

        float Lnew = Mathf.CeilToInt((180 - angle) / sectorsNew) - 1;
        if (Lnew < 0) Lnew += maze.cells[(int)Rnew].Length;
        if (Lnew >= maze.cells[(int)Rnew].Length) Lnew -= maze.cells[(int)Rnew].Length;
        return new Vector2Int((int)Rnew, (int)Lnew);
    }

    public List<Vector3> DrawPath(InstalateMazeRound MazeSpawner, Vector2 startPosition, Vector2 finishPosition)
    {
        MazeRound maze = MazeSpawner.maze;

        finishPosition = calculatoinPos(maze, finishPosition);
        startPosition = calculatoinPos(maze, startPosition);


        //(R + 0.5f) * Mathf.Sin(Mathf.Deg2Rad * (L * sectors + sectors / 2f) = Pos.y

        //return new List<Vector3>();
        List<bool[]> Visited = new List<bool[]>();
        List<int[]> DistanceFromStart = new List<int[]>();

        for (int p = 0; p < maze.cells.Count; p++)
        {
            float degree = Mathf.CeilToInt(Mathf.Log(p + 2, 2)) - 1;

            bool[] visited = new bool[maze.cells[p].Length];
            Visited.Add(visited);

            int[] distanceFromStart = new int[maze.cells[p].Length];
            DistanceFromStart.Add(distanceFromStart);
        }

        MazeCellRound currentCell = maze.cells[(int)startPosition.x][(int)startPosition.y];
        DistanceFromStart[(int)startPosition.x][(int)startPosition.y] = 1;

        List<MazeCellRound> mazeCells = new List<MazeCellRound>();
        mazeCells.Add(currentCell);

        while (mazeCells.Count > 0)
        {
            List<MazeCellRound> newMazeCells = new List<MazeCellRound>();
            foreach (MazeCellRound mazeCell in mazeCells)
            {

                if (mazeCell.R < maze.cells.Count - 2)//навправление от центра
                    if (Mathf.CeilToInt(Mathf.Log(mazeCell.R + 2, 2)) < Mathf.CeilToInt(Mathf.Log(mazeCell.R + 3, 2)))
                    {
                        if (!maze.cells[mazeCell.R + 1][mazeCell.L * 2].WallTop && !Visited[mazeCell.R + 1][mazeCell.L * 2])
                        {
                            newMazeCells.Add(maze.cells[mazeCell.R + 1][mazeCell.L * 2]);
                            Visited[mazeCell.R + 1][mazeCell.L * 2] = true;
                            DistanceFromStart[mazeCell.R + 1][mazeCell.L * 2] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                        }
                        
                        if (!maze.cells[mazeCell.R + 1][mazeCell.L * 2 + 1].WallTop && !Visited[mazeCell.R + 1][mazeCell.L * 2 + 1])
                        {
                            newMazeCells.Add(maze.cells[mazeCell.R + 1][mazeCell.L * 2 + 1]);
                            Visited[mazeCell.R + 1][mazeCell.L * 2 + 1] = true;
                            DistanceFromStart[mazeCell.R + 1][mazeCell.L * 2 + 1] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                        }
                    }
                    else
                    {
                        if (!maze.cells[mazeCell.R + 1][mazeCell.L].WallTop && !Visited[mazeCell.R + 1][mazeCell.L])
                        {
                            newMazeCells.Add(maze.cells[mazeCell.R + 1][mazeCell.L]);
                            Visited[mazeCell.R + 1][mazeCell.L] = true;
                            DistanceFromStart[mazeCell.R + 1][mazeCell.L] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                        }
                    }

                if (mazeCell.R > 0)//навправление в центр
                    if (Mathf.CeilToInt(Mathf.Log(mazeCell.R + 2, 2)) > Mathf.CeilToInt(Mathf.Log(mazeCell.R + 1, 2)))
                    {
                        if (!mazeCell.WallTop && !Visited[mazeCell.R - 1][Mathf.FloorToInt(mazeCell.L / 2f)])
                        {
                            newMazeCells.Add(maze.cells[mazeCell.R - 1][Mathf.FloorToInt(mazeCell.L / 2f)]);
                            Visited[mazeCell.R - 1][Mathf.FloorToInt(mazeCell.L / 2f)] = true;
                            DistanceFromStart[mazeCell.R - 1][Mathf.FloorToInt(mazeCell.L / 2f)] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                        }
                    }
                    else
                    {
                        if (!mazeCell.WallTop && !Visited[mazeCell.R - 1][mazeCell.L])
                        {
                            newMazeCells.Add(maze.cells[mazeCell.R - 1][mazeCell.L]);
                            Visited[mazeCell.R - 1][mazeCell.L] = true;
                            DistanceFromStart[mazeCell.R - 1][mazeCell.L] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                        }
                    }

                if (!maze.cells[mazeCell.R][(mazeCell.L < maze.cells[mazeCell.R].Length - 1) ? (mazeCell.L + 1) : 0].WallRigth && !Visited[mazeCell.R][(mazeCell.L < maze.cells[mazeCell.R].Length - 1) ? (mazeCell.L + 1) : 0])
                {
                    newMazeCells.Add(maze.cells[mazeCell.R][(mazeCell.L < maze.cells[mazeCell.R].Length - 1) ? (mazeCell.L + 1) : 0]);
                    Visited[mazeCell.R][(mazeCell.L < maze.cells[mazeCell.R].Length - 1) ? (mazeCell.L + 1) : 0] = true;
                    DistanceFromStart[mazeCell.R][(mazeCell.L < maze.cells[mazeCell.R].Length - 1) ? (mazeCell.L + 1) : 0] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                }
                
                if (!mazeCell.WallRigth && !Visited[mazeCell.R][(mazeCell.L > 0) ? (mazeCell.L - 1) : maze.cells[mazeCell.R].Length - 1])
                {
                    newMazeCells.Add(maze.cells[mazeCell.R][(mazeCell.L > 0) ? (mazeCell.L - 1) : maze.cells[mazeCell.R].Length - 1]);
                    Visited[mazeCell.R][(mazeCell.L > 0) ? (mazeCell.L - 1) : maze.cells[mazeCell.R].Length - 1] = true;
                    DistanceFromStart[mazeCell.R][(mazeCell.L > 0) ? (mazeCell.L - 1) : maze.cells[mazeCell.R].Length - 1] = DistanceFromStart[mazeCell.R][mazeCell.L] + 1;
                }
            }

            mazeCells = newMazeCells;

            foreach (MazeCellRound mazeCell in mazeCells)
            {
                if (mazeCell.R == (int)finishPosition.x && mazeCell.L == (int)finishPosition.y)
                {
                    mazeCells = new List<MazeCellRound>();
                    break;
                }
            }
        }


        int r = (int)finishPosition.x;
        int l = (int)finishPosition.y;

        List<Vector3> positions = new List<Vector3>();

        currentCell = maze.cells[r][l];

        float sectors;
        Vector3 pos;

        while (DistanceFromStart[currentCell.R][currentCell.L] > 2 && positions.Count < int.MaxValue)
        {
            sectors = 360f / maze.cells[r].Length;
            pos = new Vector3(
                (r + 0.5f) * Mathf.Cos(Mathf.Deg2Rad * (180 - l * sectors - sectors / 2f)),
                0, (r + 0.5f) * Mathf.Sin(Mathf.Deg2Rad * (l * sectors + sectors / 2f)));


            positions.Add(pos);
            currentCell = maze.cells[r][l];

            bool step = false;

            if (!step)
                if (r < maze.cells.Count - 2)//навправление от центра
                    if (Mathf.CeilToInt(Mathf.Log(r + 2, 2)) < Mathf.CeilToInt(Mathf.Log(r + 3, 2)))
                    {
                        if (!maze.cells[r + 1][l * 2].WallTop && DistanceFromStart[r + 1][l * 2] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r + 1][l * 2] != 0)
                        {
                            step = true;
                            l = 2 * l;
                            r++;
                        }
                        else if (!maze.cells[r + 1][l * 2 + 1].WallTop && DistanceFromStart[r + 1][l * 2 + 1] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r + 1][l * 2 + 1] != 0)
                        {
                            step = true;
                            l = 2 * l + 1;
                            r++;
                        }
                    }
                    else
                    {
                        if (!maze.cells[r + 1][l].WallTop && DistanceFromStart[r + 1][l] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r + 1][l] != 0)
                        {
                            step = true;
                            r++;
                        }
                    }

            if (!step)
                if (r > 0)//навправление в центр
                    if (Mathf.CeilToInt(Mathf.Log(r + 2, 2)) > Mathf.CeilToInt(Mathf.Log(r + 1, 2)))
                    {
                        if (!currentCell.WallTop && DistanceFromStart[r - 1][Mathf.FloorToInt(l / 2f)] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r - 1][Mathf.FloorToInt(l / 2f)] != 0)
                        {
                            step = true;
                            l = Mathf.FloorToInt(l / 2f);
                            r--;
                        }
                    }
                    else
                    {
                        if (!currentCell.WallTop && DistanceFromStart[r - 1][l] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r - 1][l] != 0)
                        {
                            step = true;
                            r--;
                        }
                    }

            if (!step)
                if (!maze.cells[r][(l < maze.cells[r].Length - 1) ? (l + 1) : 0].WallRigth &&
                    DistanceFromStart[r][(l < maze.cells[r].Length - 1) ? (l + 1) : 0] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r][(l < maze.cells[r].Length - 1) ? (l + 1) : 0] != 0) l++;
                else if (!currentCell.WallRigth &&
                    DistanceFromStart[r][(l > 0) ? (l - 1) : maze.cells[r].Length - 1] < DistanceFromStart[currentCell.R][currentCell.L] && DistanceFromStart[r][(l > 0) ? (l - 1) : maze.cells[r].Length - 1] != 0) l--;


            if (l >= maze.cells[r].Length) l = 0;
            else if (l < 0) l = maze.cells[r].Length - 1;
        }

        sectors = 360f / maze.cells[r].Length;
        pos = new Vector3((r + 0.5f) * Mathf.Cos(Mathf.Deg2Rad * (180 - l * sectors - sectors / 2f)),
            0, (r + 0.5f) * Mathf.Sin(Mathf.Deg2Rad * (l * sectors + sectors / 2f)));

        positions.Add(pos);
        componentLineRenderer.positionCount = positions.Count;
        componentLineRenderer.SetPositions(positions.ToArray());


        return positions;
    }
}
