using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public LineRenderer componentLineRenderer;

    void Awake()
    {
        componentLineRenderer = GetComponent<LineRenderer>();
    }

    public List<Vector3> DrawPath(InstalateMaze MazeSpawner, Vector2 startPosition, Vector2Int finishPosition)
    {
        Maze maze = MazeSpawner.maze;

        bool[,] Visited = new bool[maze.cells.GetLength(0), maze.cells.GetLength(1)];
        int[,] DistanceFromStart = new int[maze.cells.GetLength(0), maze.cells.GetLength(1)];

        MazeCell currentCell = maze.cells[(int)startPosition.x, (int)startPosition.y];
        DistanceFromStart[(int)startPosition.x, (int)startPosition.y] = 1;

        List<MazeCell> mazeCells = new List<MazeCell>();
        mazeCells.Add(currentCell);

        while (mazeCells.Count > 0)
        {
            List<MazeCell> newMazeCells = new List<MazeCell>();
            foreach (MazeCell mazeCell in mazeCells)
            {
                if (mazeCell.X > 0 && !mazeCell.WallLeft && !Visited[mazeCell.X - 1, mazeCell.Y])
                {
                    newMazeCells.Add(maze.cells[mazeCell.X - 1, mazeCell.Y]);
                    Visited[mazeCell.X - 1, mazeCell.Y] = true;
                    DistanceFromStart[mazeCell.X - 1, mazeCell.Y] = DistanceFromStart[mazeCell.X, mazeCell.Y] + 1;
                }

                if (mazeCell.X < maze.cells.GetLength(0) - 1 && !maze.cells[mazeCell.X + 1, mazeCell.Y].WallLeft && !Visited[mazeCell.X + 1, mazeCell.Y])
                {
                    newMazeCells.Add(maze.cells[mazeCell.X + 1, mazeCell.Y]);
                    Visited[mazeCell.X + 1, mazeCell.Y] = true;
                    DistanceFromStart[mazeCell.X + 1, mazeCell.Y] = DistanceFromStart[mazeCell.X, mazeCell.Y] + 1;
                }

                if (mazeCell.Y > 0 && !mazeCell.WallBottom && !Visited[mazeCell.X, mazeCell.Y - 1])
                {
                    newMazeCells.Add(maze.cells[mazeCell.X, mazeCell.Y - 1]);
                    Visited[mazeCell.X, mazeCell.Y - 1] = true;
                    DistanceFromStart[mazeCell.X, mazeCell.Y - 1] = DistanceFromStart[mazeCell.X, mazeCell.Y] + 1;
                }

                if (mazeCell.Y < maze.cells.GetLength(1) - 1 && !maze.cells[mazeCell.X, mazeCell.Y + 1].WallBottom && !Visited[mazeCell.X, mazeCell.Y + 1])
                {
                    newMazeCells.Add(maze.cells[mazeCell.X, mazeCell.Y + 1]);
                    Visited[mazeCell.X, mazeCell.Y + 1] = true;
                    DistanceFromStart[mazeCell.X, mazeCell.Y + 1] = DistanceFromStart[mazeCell.X, mazeCell.Y] + 1;
                }
            }
            mazeCells = newMazeCells;

            foreach (MazeCell mazeCell in mazeCells)
            {
                if (mazeCell.X == finishPosition.x && mazeCell.Y == finishPosition.y)
                {
                    mazeCells = new List<MazeCell>();
                    break;
                }
            }
        }


        int x = finishPosition.x;
        int y = finishPosition.y;
        List<Vector3> positions = new List<Vector3>();
        currentCell = maze.cells[x, y];
        MazeCell nextCells;

        positions.Add(new Vector3(x + 0.5f, 0, y + 0.5f));

        while (DistanceFromStart[x, y] > 2 && positions.Count < int.MaxValue)
        {
            currentCell = maze.cells[x, y];
            nextCells = currentCell;

            if (x > 0 && !currentCell.WallLeft && DistanceFromStart[x - 1, y] != 0 &&
              DistanceFromStart[x - 1, y] < DistanceFromStart[currentCell.X, currentCell.Y])
            {
                if (DistanceFromStart[x - 1, y] < DistanceFromStart[nextCells.X, nextCells.Y])
                    nextCells = maze.cells[x - 1, y];
            }

            if (x < maze.cells.GetLength(0) - 1 && !maze.cells[x + 1, y].WallLeft && DistanceFromStart[x + 1, y] != 0 &&
                DistanceFromStart[x + 1, y] < DistanceFromStart[currentCell.X, currentCell.Y])
            {
                if (DistanceFromStart[x + 1, y] < DistanceFromStart[nextCells.X, nextCells.Y])
                    nextCells = maze.cells[x + 1, y];
            }

            if (y > 0 && !currentCell.WallBottom && DistanceFromStart[x, y - 1] != 0 &&
               DistanceFromStart[x, y - 1] < DistanceFromStart[currentCell.X, currentCell.Y])
            {
                if (DistanceFromStart[x, y - 1] < DistanceFromStart[nextCells.X, nextCells.Y])
                    nextCells = maze.cells[x, y - 1];
            }

            if (y < maze.cells.GetLength(1) - 1 && !maze.cells[x, y + 1].WallBottom && DistanceFromStart[x, y + 1] != 0 &&
                DistanceFromStart[x, y + 1] < DistanceFromStart[currentCell.X, currentCell.Y])
            {
                if (DistanceFromStart[x, y + 1] < DistanceFromStart[nextCells.X, nextCells.Y])
                    nextCells = maze.cells[x, y + 1];
            }

            x = nextCells.X;
            y = nextCells.Y;

            positions.Add(new Vector3(x + 0.5f, 0, y + 0.5f));
        }

        positions.Add(new Vector3(startPosition.x, 0, startPosition.y));

        componentLineRenderer.positionCount = positions.Count;
        componentLineRenderer.SetPositions(positions.ToArray());

        return positions;
    }
}
