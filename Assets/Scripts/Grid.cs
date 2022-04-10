using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    
    public static Grid Instance;
    private Tiles[,] grid;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private Vector2 spacing;
    [SerializeField]
    private Vector2 offset;

    private int minesLimit;

    private GridView gridView;
    
    
    public Tiles tilePrefab;
    public Transform parent;
    
    void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        grid = new Tiles[width, height];
        gridView = GetComponent<GridView>();
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = Instantiate(tilePrefab, parent);
                tile.Initialize(x, y);
                grid[x, y] = tile;
            }
        }
        minesLimit = (width * height) / 5;
        GenerateMines();
        GenerateNumbers();
        gridView.GenerateGrid(grid, spacing, offset);
        HideAllTiles();
        //DebugGrid();
    }
    
    //debug grid
    void DebugGrid()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Debug.Log($"{grid[x, y].X}, {grid[x, y].Y} - {grid[x, y].mineType} - {grid[x, y].MineCount}");
    }
    
    //algorith to generate the mines randomly
    private void GenerateMines()
    {
        for (int i = 0; i < minesLimit; i++)
        {
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);
            grid[x, y].mineType = MineType.MINE;
        }
    }
    
    //generate the numbers of mines around the tiles
    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].mineType != MineType.MINE)
                {
                    int mines = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (x + i >= 0 && x + i < width && y + j >= 0 && y + j < height)
                            {
                                if (grid[x + i, y + j].mineType == MineType.MINE)
                                    mines++;
                            }
                        }
                    }

                    if (mines != 0)
                    {
                        grid[x, y].mineType = MineType.NUMBER;
                        grid[x, y].MineCount = mines;
                    }
                    else
                    {
                        grid[x, y].mineType = MineType.EMPTY;
                        grid[x, y].MineCount = mines;
                    }
                }
            }
        }
    }
    
    public void HideTile(int x, int y) => gridView.HideTile(grid[x, y]);

    public void HideAllTiles()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                HideTile(x,y);
    }
    
    public void SetFlag(int x, int y) => gridView.SetFlag(grid[x, y]);
    
    public void RemoveFlag(int x, int y) => gridView.SetNormal(grid[x, y]);
    
    
    //clear all empty tiles around the tile that was clicked and put IsShowed = true to avoid the tile to be clicked again without recursion
    public void ClearEmptyTiles(int x, int y)
    {
        if (grid[x, y].mineType == MineType.NUMBER)
            return;
        grid[x, y].isShowed = true;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (x + i >= 0 && x + i < width && y + j >= 0 && y + j < height)
                {
                    if (grid[x + i, y + j].mineType == MineType.EMPTY && !grid[x + i, y + j].isShowed)
                    {
                        grid[x + i, y + j].isShowed = true;
                        gridView.ShowTile(grid[x + i, y + j]);
                        ClearEmptyTiles(x + i, y + j);
                    }
                }
            }
        }
    }

    public void ShowTile(int x, int y) => gridView.ShowTile(grid[x, y],false);

    public void ShowAllTiles() => gridView.ShowAllTiles(grid);

    public void GameOver()
    {
        Debug.Log("Game Over");
        ShowAllTiles();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                Gizmos.DrawWireCube(new Vector3(x, y, 0) * spacing + offset, new Vector3(spacing.x, spacing.y, 1));
    }
}
