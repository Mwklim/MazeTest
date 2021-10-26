using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalateMazeRound : MonoBehaviour
{
    public CellRound CellPrefab;

    public MazeRound maze;

    List<CellRound> cells = new List<CellRound>();

    public void MazeGeneration(Vector2Round sizeMaze, Vector2Round startCell)
    {
        GeneratorMazeRound generatorMaze = new GeneratorMazeRound();

        maze = generatorMaze.MazeGeneration(sizeMaze, startCell);

        foreach (CellRound cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells = new List<CellRound>();

        for (int r = 0; r < maze.cells.Count; r++)
        {
            for (int l = 0; l < maze.cells[r].Length; l++)
            {
                float sectors = 360f / maze.cells[r].Length;
                float perimeterSector = (1f * 2f * Mathf.PI * (r + 0.05f) / maze.cells[r].Length);// / Mathf.Cos(Mathf.Deg2Rad * sectors / Mathf.Pow(2, r + 2));

                Vector3 pos = new Vector3(r * Mathf.Cos(Mathf.Deg2Rad * (180 - l * sectors)),
                          0, r * Mathf.Sin(Mathf.Deg2Rad * (l * sectors)));

                CellRound c = Instantiate(CellPrefab, pos, Quaternion.Euler(0, l * sectors, 0));

                c.WallTop.transform.localScale = new Vector3(perimeterSector, c.WallTop.transform.localScale.y, c.WallTop.transform.localScale.z);
                c.WallTop.transform.localRotation = Quaternion.Euler(0, 90f + sectors / (2.04f * Mathf.Cos(Mathf.Deg2Rad * sectors)), 0);
                c.WallTop.transform.localPosition = new Vector3(
                    perimeterSector * Mathf.Sin(Mathf.Deg2Rad * (sectors / 2f)) / 2f,
                    c.WallTop.transform.localPosition.y,
                    perimeterSector * Mathf.Cos(Mathf.Deg2Rad * (sectors / 2f)) / 2f);

                if (r < maze.cells.Count - 1)
                {
                    float perimeterFloor = 1.1f * 2f * Mathf.PI * (r + 1) / maze.cells[r].Length;

                    c.Floor.transform.localScale = new Vector3(perimeterFloor, c.Floor.transform.localScale.y, c.Floor.transform.localScale.z);
                    c.Floor.transform.localRotation = Quaternion.Euler(0, 90f + sectors / 2f, 0);
                    c.Floor.transform.localPosition = new Vector3(
                        c.Floor.transform.localPosition.x,
                        c.Floor.transform.localPosition.y,
                        perimeterFloor * Mathf.Cos(Mathf.Deg2Rad * (sectors / 2f)) / 2.2f);
                }

                c.WallRigth.SetActive(maze.cells[r][l].WallRigth);
                c.WallTop.SetActive(maze.cells[r][l].WallTop);
                c.Floor.SetActive(maze.cells[r][l].Floor);

                cells.Add(c);
            }
        }
    }
    public void DestroyMaze()
    {
        foreach (CellRound cell in cells)
        {
            Destroy(cell.gameObject);
        }
        cells = new List<CellRound>();
    }
}
