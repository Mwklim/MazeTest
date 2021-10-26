using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game game;

    public Camera camera;

    public InstalateMaze instalateMaze;
    public InstalateMazeRound instalateMazeRound;

    public GameObject pathFinding;
    public Player player;

    int sizeX = 10;
    int sizeY = 10;

    public Text[] textSize;
    public Text[] TitleSize;
    public Text[] TitleTypeMaze;

    public TypeMaze typeMaze = TypeMaze.Rectangular;
    TypeMaze nowtypeMaze = TypeMaze.Rectangular;


    void Start()
    {
        game = this;

        SetTypeMaze((int)typeMaze);
        MazeGeneration();
    }

    private void SetTitleSize()
    {
        switch (typeMaze)
        {
            case TypeMaze.Rectangular:
                TitleSize[0].text = "Ширина";
                TitleSize[1].text = "Высота";
                break;
            case TypeMaze.Round:
                TitleSize[0].text = "Радиус";
                TitleSize[1].text = "Сегментов в центре";
                break;
        }
    }

    public void MazeGeneration()
    {
        instalateMaze.DestroyMaze();
        instalateMazeRound.DestroyMaze();

        switch (typeMaze)
        {
            case TypeMaze.Rectangular:
                Vector2Int sizeMaze = new Vector2Int(sizeX + 1, sizeY + 1);
                instalateMaze.MazeGeneration(sizeMaze, new Vector2Int(0, 0));
                camera.transform.position = new Vector3((sizeMaze.x - 1) / 2f, 20f, 0.5f * (sizeMaze.y - 1) / 2f);
                camera.orthographicSize = Mathf.Max(sizeMaze.x * Screen.height / Screen.width, 1.5f * sizeMaze.y) / 2f;

                player.transform.position = new Vector3(Random.Range(0, sizeMaze.x - 1) + 0.5f, 0, Random.Range(0, sizeMaze.y - 1) + 0.5f);
                break;
            case TypeMaze.Round:
                Vector2Round sizeMazeRound = new Vector2Round(sizeX + 1, sizeY);
                instalateMazeRound.MazeGeneration(sizeMazeRound, new Vector2Round(0, 0));
                camera.transform.position = new Vector3(0, 20f, -(sizeMazeRound.r - 1) / 2f);
                camera.orthographicSize = (sizeMazeRound.r) * 2.1f;

                int R = Random.Range(0, instalateMazeRound.maze.cells.Count - 1);
                int L = Random.Range(0, instalateMazeRound.maze.cells[R].Length);
                float sectors = 360f / instalateMazeRound.maze.cells[R].Length;
                player.transform.position = new Vector3((R + 0.5f) * Mathf.Cos(Mathf.Deg2Rad * (180 - L * sectors - sectors / 2f)), 0, (R + 0.5f) * Mathf.Sin(Mathf.Deg2Rad * (L * sectors + sectors / 2f)));
                break;
        }

        if (pathFinding.GetComponent<PathFinding>().componentLineRenderer != null) pathFinding.GetComponent<PathFinding>().componentLineRenderer.positionCount = 0;
        if (pathFinding.GetComponent<PathFindingRound>().componentLineRenderer != null) pathFinding.GetComponent<PathFindingRound>().componentLineRenderer.positionCount = 0;

        player.SetPosition(new List<Vector3>(), null);

        nowtypeMaze = typeMaze;
    }

    public void DrawPath(Vector2 finishPosition)
    {
        List<Vector3> way;
        switch (nowtypeMaze)
        {
            case TypeMaze.Rectangular:
                way = pathFinding.GetComponent<PathFinding>().DrawPath(instalateMaze, new Vector2(player.transform.position.x, player.transform.position.z), new Vector2Int((int)finishPosition.x, (int)finishPosition.y));
                player.SetPosition(way, pathFinding.GetComponent<PathFinding>().componentLineRenderer);
                break;
            case TypeMaze.Round:
                way = pathFinding.GetComponent<PathFindingRound>().DrawPath(instalateMazeRound, new Vector2(player.transform.position.x, player.transform.position.z), finishPosition);
                player.SetPosition(way, pathFinding.GetComponent<PathFindingRound>().componentLineRenderer);
                break;
        }
    }

    public void EditSizeX(int size)
    {
        sizeX += size;
        if (sizeX < 2)
            sizeX = 2;
        textSize[0].text = sizeX.ToString();
    }

    public void EditSizeY(int size)
    {
        sizeY += size;
        if (typeMaze == TypeMaze.Rectangular && sizeY < 2)
            sizeY = 2;
        else if (typeMaze == TypeMaze.Round)
            if (sizeY < 6)
                sizeY = 6;
            else if (sizeY > 10)
                sizeY = 10;
        textSize[1].text = sizeY.ToString();
    }

    public void SetTypeMaze(int type)
    {
        typeMaze = (TypeMaze)type;
        EditSizeX(0);
        EditSizeY(0);

        SetTitleSize();

        for (int i = 0; i < TitleTypeMaze.Length; i++)
        {
            int a = (i == type) ? 0 : 1;
            TitleTypeMaze[i].color = new Color(a, a, a, 1f);
        }
    }
}

public enum TypeMaze
{
    Rectangular,
    Round
}
