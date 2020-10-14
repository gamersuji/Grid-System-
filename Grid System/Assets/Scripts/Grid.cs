using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Grid 
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }


    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private Vector3 originPosition;
    private TextMesh[,] debugTextArray;


    /// <summary>
    /// Helps initialize the Grid
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    public Grid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //Debug.Log(x + ", " + y);
                debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize,cellSize)*.5f, 20,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y+1),Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);

            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

    }

    public Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    /// <summary>
    /// Help us point to an array index using the mouse position, help us to both get and set value into that particular Index
    /// </summary>
    /// <param name="mousePos"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void GetXY(Vector3 mousePos, out int x, out int y)
    {
        ///we divide it by cell size cause the texts that appear are stored by an offset which is a bit 
        ///away from the actual grid position, Also orgin position is added to the actual text 
        ///position while it has been set, so when the user taps on it, the actual index of the 
        ///text he is trying to acess will also be further away, so we will reduce it from the 
        ///mouse position to identify the index, so we can retrieve/add value
        
        x = Mathf.FloorToInt((mousePos  - originPosition).x/ cellSize);
        y = Mathf.FloorToInt((mousePos - originPosition).y/ cellSize);
    }

    /// <summary>
    ///Set values into appropriate array's, The value is set based on the data's position as the index 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    private void SetValue(int x, int y, int value)
    {
        if(x>=0 && y>=0 && x<width && y<height)
        {
            ///sets the data into an array based on it's position as the index
            gridArray[x, y] = Mathf.Clamp(value,HEAT_MAP_MIN_VALUE,HEAT_MAP_MAX_VALUE);
            ///we do the same here but save TextMeshs'
            debugTextArray[x, y].text = gridArray[x, y].ToString();

            if (OnGridValueChanged != null) OnGridValueChanged(this,new OnGridValueChangedEventArgs {x=x,y=y});
        }
    }

    /// <summary>
    /// Use the mouse position as the Index to save data
    /// </summary>
    /// <param name="mousePos"></param>
    /// <param name="value"></param>
    public void SetValue(Vector3 mousePos,int value)
    {
        int x, y;
        GetXY(mousePos, out x, out y);
        SetValue(x, y, value);
    }

    /// <summary>
    /// Used to get value based on the positional index provided
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
           return gridArray[x, y];
        
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    ///  Use the mouse position as the Index to retrieve data
    /// </summary>
    /// <param name="mousePos"></param>
    /// <returns></returns>
    public int GetValue(Vector3 mousePos)
    {
        int x, y;
        GetXY(mousePos,out x, out y);
        return GetValue(x,y);
    }

    public void AddValue(int x,int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value); //we need to add value every time we click a block
    }

    //public void AddValue(Vector3 worldPosition,int value, int range)
    //{
    //    GetXY(worldPosition,out int originX, out int originY);

    //    for (int x = 0; x < range; x++)
    //    {
    //        for (int y = 0; y < range-x; y++)///range-x creates a stair
    //        {
    //            AddValue(originX + x, originY + y, value); 

    //            ///putting the stairs together to form a diamond
    //            if(x!=0)///this will prevent adding value to overlaping areas while forming the diamond
    //            {
    //                AddValue(originX - x, originY + y, value);
    //            }
    //            if (y != 0)///this will prevent adding value to overlaping areas while forming the diamond
    //            {
    //                AddValue(originX + x, originY - y, value);
    //                if (x != 0)///this will prevent adding value to overlaping areas while forming the diamond
    //                {
    //                    AddValue(originX - x, originY - y, value);
    //                }
    //            }

    //        }
    //    }
    //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="value"></param>
        /// <param name="fullValueRange">This is the range where the value is the same and in the maximum</param>
        /// <param name="totalRange">Is the range where the data will begin to reduce</param>
    public void AddValue(Vector3 worldPosition, int value, int fullValueRange,int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
        GetXY(worldPosition, out int originX, out int originY);

        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)///range-x creates a stair
            {

                int radius = x + y;
                int addValueAmount = value;
                if(radius>fullValueRange)//when you exceed the full value range the value will start decreasing
                {
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }

                AddValue(originX + x, originY + y, addValueAmount);

                ///putting the stairs together to form a diamond
                if (x != 0)///this will prevent adding value to overlaping areas while forming the diamond
                {
                    AddValue(originX - x, originY + y, addValueAmount);
                }
                if (y != 0)///this will prevent adding value to overlaping areas while forming the diamond
                {
                    AddValue(originX + x, originY - y, addValueAmount);
                    if (x != 0)///this will prevent adding value to overlaping areas while forming the diamond
                    {
                        AddValue(originX - x, originY - y, addValueAmount);
                    }
                }

            }
        }
    }


    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    //public Vector3 GetWorldPosition(int x, int y)
    //{
    //    return new Vector3(x, y) * cellSize + originPosition;
    //}
}
