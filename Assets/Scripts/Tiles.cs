using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tiles : MonoBehaviour, IPointerClickHandler
{
    private int x;
    private int y;
    public MineType mineType;
    private int mineCount;
    public bool isShowed;
    public bool isFlag;
    public int X => x;
    public int Y => y;
    public int MineCount
    {
        get => mineCount;
        set => mineCount = value;
    }

    //constructor
    public Tiles(int x, int y, MineType mineType)
    {
        this.x = x;
        this.y = y;
        this.mineType = mineType;
        isShowed = false;
        isFlag = false;
    }
    public Tiles(int x, int y)
    {
        this.x = x;
        this.y = y;
        isShowed = false;
    }

    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
        isShowed = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isShowed)
            return;
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if(isFlag)
                    return;
                Grid.Instance.ShowTile(x, y);
                isShowed = true;
                break;
            case PointerEventData.InputButton.Right:
                if (!isFlag)
                {
                    isFlag = true;
                    Grid.Instance.SetFlag(x,y);   
                }
                else
                {
                    isFlag = false;
                    Grid.Instance.RemoveFlag(x, y);
                }
                break;
        }
    }
}

public enum MineType
{
    EMPTY,
    MINE,
    NUMBER,
    FLAG,
    QUESTION
}
