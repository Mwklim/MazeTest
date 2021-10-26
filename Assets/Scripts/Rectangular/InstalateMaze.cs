using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalateMaze : MonoBehaviour
{
    public Cell CellPrefab;

    public Maze maze;

    List<Cell> cells = new List<Cell>();

    public void MazeGeneration(Vector2Int sizeMaze, Vector2Int startCell)
    {
        GeneratorMaze generatorMaze = new GeneratorMaze();
        maze = generatorMaze.MazeGeneration(sizeMaze, startCell);

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x, 0, y), Quaternion.identity);
                c.transform.parent = gameObject.transform;

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
                if (c.Floor != null) c.Floor.SetActive(maze.cells[x, y].Floor);
                cells.Add(c);
            }
        }
    }

    public void DestroyMaze()
    {
        foreach (Cell cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells = new List<Cell>();
    }
}
