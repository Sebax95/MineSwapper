using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{ 
    public void GenerateGrid(Tiles[,] grid, Vector2 spacing,Vector2 offset)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var tile = grid[x, y];
                tile.transform.localPosition = new Vector3(grid[x, y].X * spacing.x + offset.x, grid[x, y].Y * spacing.y + offset.y, 0);
                tile.name = "Tile " + x + "," + y;
                ShowTile(grid[x,y], true);
            }
        }
    }

    public void HideTile(Tiles tile)
    {
        var text = tile.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "";
    }
    
    public void ShowTile(Tiles tile, bool isStarting = false)
    {
        var text = tile.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        switch (tile.mineType)
        {
            case MineType.MINE:
                text.text = "M";
                break;
            case MineType.EMPTY:
                text.text = "";
                break;
            case MineType.NUMBER:
                if(tile.MineCount == 2)
                    text.color = Color.cyan;
                else if(tile.MineCount > 3)
                    text.color = Color.red;
                else
                    text.color = Color.white;
                text.text = tile.MineCount.ToString();
                break;
            default:
                text.text = text.text;
                break;
        }

        if (isStarting)
            return;
        if(tile.mineType == MineType.MINE)
            Grid.Instance.GameOver();
        else if(tile.mineType == MineType.EMPTY)
        {
            tile.GetComponent<Image>().color = Color.gray;
            Grid.Instance.ClearEmptyTiles(tile.X, tile.Y);
        }
    }

    public void SetFlag(Tiles tile)
    {
        tile.isFlag = true;
        tile.GetComponent<Image>().color = Color.yellow;
        tile.GetComponentInChildren<TextMeshProUGUI>().text = "F";
        tile.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void SetNormal(Tiles tile)
    {
        tile.isFlag = false;
        tile.GetComponent<Image>().color = Color.black;
        tile.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    public void ShowEmpty(Tiles tile)
    {
        if(tile.mineType == MineType.EMPTY)
            tile.GetComponent<Image>().color = Color.gray;
    }
    
    public void ShowAllTiles(Tiles[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var tile = grid[x, y];
                var text = tile.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                switch (tile.mineType)
                {
                    case MineType.MINE: 
                        text.text = "M";
                        break;
                    case MineType.EMPTY:
                        text.text = "";
                        break;
                    case MineType.NUMBER:
                        tile.GetComponent<Image>().color = Color.black;
                        if(tile.MineCount == 2)
                            text.color = Color.cyan;
                        else if(tile.MineCount >= 3)
                            text.color = Color.red;
                        else
                            text.color = Color.white;
                        text.text = tile.MineCount.ToString();
                        break;
                    default:
                        text.text = text.text;
                        break;
                }
                if(tile.mineType == MineType.EMPTY)
                    tile.GetComponent<Image>().color = Color.gray;
            }
        }
        
    }
}