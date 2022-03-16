using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RuntimeSceneSerialization;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GridBuilder : MonoBehaviour
{
    //GridSettings
    [SerializeField] private int gridHeight, gridWidth, gridLength;
    [SerializeField] private GameObject gridPrefab;


    private List<Vector3Int> gridPositions = new List<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> gridTiles = new Dictionary<Vector3Int, GameObject>();
    private Controls controls;

    private void Awake()
    {
        //Generate grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridLength; z++)
                {
                    gridPositions.Add(new Vector3Int(x, y, z));
                }
            }
        }

        //Create parent for grid to organize hierarchy
        GameObject grid = new GameObject();
        grid.name = "grid";

        //Set settings on control
        controls = GetComponent<Controls>();
        controls.gridTiles = gridTiles;
        controls.gridScale = new Vector3Int(gridWidth, gridHeight, gridLength);
    }

}
