using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class HeatMapVisual : MonoBehaviour
{
    private Grid grid;
    private Mesh mesh;
    private bool updateMesh;
    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        

    }


    public void SetGrid(Grid grid)
    {
        this.grid = grid;
        UpdateTheHeatMapVisuals();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;

    }

    private void Grid_OnGridValueChanged(object sender, System.EventArgs e)
    {
        //UpdateTheHeatMapVisuals();
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh) ///This way we stop the event for firing the mesh update every time the player clicks, instead we only fire at the end of the update
        {
            updateMesh = false;
            UpdateTheHeatMapVisuals();
        }
    }

    // Update is called once per frame
    private void UpdateTheHeatMapVisuals()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                int gridValue = grid.GetValue(x, y);
                ///This will help in the transition of colors also we need normalised data because
                ///UV only operates as normalised value
                float gridValueNormalised = (float)gridValue / Grid.HEAT_MAP_MAX_VALUE;
                ///IDK how y axis is 0 here, I thought both X and Y axis is neededto create UV, it's because the color we are trying to pick the texture has data only in x axis ;)
                Vector2 gridValueUV = new Vector2(gridValueNormalised, 0f);
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y)+quadSize*.5f, 0f, quadSize, gridValueUV, gridValueUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
