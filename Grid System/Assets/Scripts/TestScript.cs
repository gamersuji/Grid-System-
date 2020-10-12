using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestScript : MonoBehaviour
{
    private Grid grid;
    void Start()
    {
        // grid = new Grid(4, 2,10f,new Vector3(20,0));
        //new Grid(2, 5,5f,new Vector3(0, -20));
        //new Grid(10, 10,20f, new Vector3(-100, -20));

        grid = new Grid(10, 10, 20f, new Vector3(-100, -20));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            grid.SetValue(mousePos, 56);
            
        }

        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
        
    }
}
