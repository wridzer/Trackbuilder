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
    [SerializeField] private GameObject gridPrefab, testPrefab;


    private List<Vector3Int> gridPositions = new List<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> gridTiles = new Dictionary<Vector3Int, GameObject>();
    private Controls controls;

    private void Start()
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
        GameObject grid = new GameObject();
        grid.name = "grid";
        foreach (Vector3Int gridPos in gridPositions)
        {
            GameObject gridPiece = Instantiate(gridPrefab, gridPos, new Quaternion(), grid.transform);
            gridTiles.Add(gridPos, gridPiece);
            controls = new Controls(gridTiles, gridHeight, testPrefab);
        }
    }

    private void Update()
    {
        controls.Update();
    }
}
