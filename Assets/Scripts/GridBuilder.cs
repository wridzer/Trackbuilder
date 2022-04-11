using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RuntimeSceneSerialization;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class GridBuilder : MonoBehaviour
{
    //General settings
    public string prefabsPath, exportPath;
    public GameObject ToolButtonPrefab, toolSelector;
    public Material previewMat;

    //GridSettings
    [SerializeField] private int gridHeight, gridWidth, gridLength;

    private Dictionary<Vector3, GameObject> gridTiles = new Dictionary<Vector3, GameObject>();
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
                    gridTiles.Add(new Vector3Int(x, y, z), null);
                }
            }
        }

        //Set settings on control
        controls = GetComponent<Controls>();
        controls.gridTiles = gridTiles;
        controls.gridScale = new Vector3Int(gridWidth, gridHeight, gridLength);
        controls.previewMaterial = previewMat;

        //Create states for tools
        StateMachine statemachine = new StateMachine();
        string[] files = System.IO.Directory.GetFiles(prefabsPath, "*.prefab");

        foreach (string file in files)
        {
            GameObject prefab = AssetDatabase.LoadMainAssetAtPath(Path.Combine(file)) as GameObject;

            State newState = new ToolState(typeof(PlaceObjectCommand), prefab, controls);
            statemachine.AddState(newState);
            GameObject newButton = Instantiate(ToolButtonPrefab, toolSelector.transform);
            newButton.GetComponent<ToolButton>().SetVariables(statemachine, newState);
            Texture2D buttonImage = AssetPreview.GetMiniThumbnail(prefab);
            newButton.GetComponentInChildren<RawImage>().texture = buttonImage;
        }
    }

    public void ImportGrid(string _filePath)
    {
        controls.Import(_filePath);
    }

    public void ExportGrid()
    {
        controls.Export(exportPath);
    }

}
