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
    public string assetBundleName { get; set; }
    public string exportPath { get; set; }
    public GameObject ToolButtonPrefab, toolSelector, EraseButtonPrefab;
    public Material previewMat;
    public Texture2D eraseButtonTexure;

    //GridSettings
    public int gridHeight { get; set; }
    public int gridWidth { get; set; }
    public int gridLength{ get; set; }

    private Dictionary<Vector3, GameObject> gridTiles = new Dictionary<Vector3, GameObject>();
    private List<GameObject> toolsList = new List<GameObject>();
    public Controls controls;
    private StateMachine stateMachine;

    public void BuildGrid()
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
        controls.enabled = true;

        //Create states for tools
        stateMachine = new StateMachine();
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
        Object[] bundleAssets = bundle.LoadAllAssets();

        //Add toolbutton for erase
        GameObject newEraseButton = Instantiate(EraseButtonPrefab, toolSelector.transform);
        newEraseButton.GetComponent<EraseButton>().gridBuilder = this;
        toolsList.Add(newEraseButton);

        foreach (object asset in bundleAssets)
        {
            //load prefab
            GameObject prefab = (GameObject)asset;

            //create state in statemachine
            State newState = new ToolState(typeof(PlaceObjectCommand), prefab, controls);
            stateMachine.AddState(newState);

            //create button with image
            GameObject newButton = Instantiate(ToolButtonPrefab, toolSelector.transform);
            newButton.GetComponent<ToolButton>().SetVariables(stateMachine, newState);
            Texture2D buttonImage = AssetPreview.GetAssetPreview(prefab);
            newButton.GetComponentInChildren<RawImage>().texture = buttonImage;
            toolsList.Add(newButton);
        }
    }

    public void ClearAll()
    {
        if(controls != null)
        {
            controls.ClearGrid();
            controls.enabled = false;
            stateMachine.ClearStates();
            foreach(var tool in toolsList)
            {
                Destroy(tool.gameObject);
            }
            toolsList.Clear();
        }
    }

    public void ImportGrid(string _filePath)
    {
        ClearAll();
        List <ICommand> importData = Importer.Import(_filePath, this);
        BuildGrid();
        controls.Import(importData);
    }

    public void ExportGrid()
    {
        if(exportPath == null)
        {
            string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
            exportPath = directory;
        }
        controls.Export(exportPath, this);
    }
}
