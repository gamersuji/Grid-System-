using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestScript : MonoBehaviour
{
    private Grid grid;
    [SerializeField]private HeatMapVisual heatMapVisual;
    void Start()
    {

        grid = new Grid(100, 100, 4f, Vector3.zero);
        heatMapVisual.SetGrid(grid);
            
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetMouseButton(0))
        {
            Vector3 pos = UtilsClass.GetMouseWorldPosition();
            int value = grid.GetValue(pos);
            grid.AddValue(pos,100,5,40);
        }
    }
}
