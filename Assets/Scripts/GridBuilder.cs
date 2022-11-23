using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

public class GridBuilder : MonoBehaviour
{
    // General settings
    public string assetBundlePath { get; set; }
    public string exportPath { get; set; }
    public GameObject ToolButtonPrefab, toolSelector, EraseButtonPrefab;
    public Material previewMat;
    public Texture2D eraseButtonTexure;
    public RenderTexture RTexture;

    // GridSettings
    public int gridHeight { get; set; }
    public int gridWidth { get; set; }
    public int gridLength{ get; set; }

    private Dictionary<Vector3, GameObject> gridTiles = new Dictionary<Vector3, GameObject>();
    private List<GameObject> toolsList = new List<GameObject>();
    public Controls controls;
    private StateMachine stateMachine;
    [HideInInspector] public object[] assets;
    public DialogueBox dialogueBox;

    public void BuildGrid()
    {
        // Generate grid
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

        // Set settings on control
        controls = GetComponent<Controls>();
        controls.gridTiles = gridTiles;
        controls.gridScale = new Vector3Int(gridWidth, gridHeight, gridLength);
        controls.previewMaterial = previewMat;
        controls.enabled = true;

        // Create states for tools
        stateMachine = new StateMachine();
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundlePath));
        Object[] bundleAssets = bundle.LoadAllAssets();
        assets = bundleAssets;

        // Add toolbutton for erase
        GameObject newEraseButton = Instantiate(EraseButtonPrefab, toolSelector.transform);
        newEraseButton.GetComponent<EraseButton>().gridBuilder = this;
        toolsList.Add(newEraseButton);

        // Preview image camera
        GameObject camera = new GameObject("ButtonPreviewCamera");
        camera.gameObject.AddComponent<Camera>();
        camera.transform.position = new Vector3(1000, 1000, 998);
        Camera cam = camera.GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.white;
        cam.targetTexture = RTexture;

        foreach (object asset in bundleAssets)
        {
            // Load prefab
            GameObject prefab = (GameObject)asset;

            // Create state in statemachine
            State newState = new ToolState(typeof(PlaceObjectCommand), prefab, controls);
            stateMachine.AddState(newState);

            // Create button with image
            GameObject newButton = Instantiate(ToolButtonPrefab, toolSelector.transform);
            newButton.GetComponent<ToolButton>().SetVariables(stateMachine, newState);
            if (prefab != null)
            {
                newButton.GetComponentInChildren<RawImage>().texture = GetToolPreview(prefab, cam);
            }
            toolsList.Add(newButton);
        }

        Destroy(camera);
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
        //BuildGrid();
        controls.Import(importData);
    }

    public void ExportGrid()
    {
        if(!Directory.Exists(exportPath) || exportPath == null)
        {
            dialogueBox.gameObject.SetActive(true);
            dialogueBox.MessageText.text = "Please enter a valid export path in settings";
        }
        controls.Export(exportPath, this);
    }

    private Texture2D GetToolPreview(GameObject _prefab, Camera _cam)
    {
        GameObject temp = Instantiate(_prefab, new Vector3(1000, 1000, 1000), Quaternion.identity); //spawn gameobject

        // Generate Texture
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = _cam.targetTexture;

        _cam.Render();

        Texture2D Image = new Texture2D(_cam.targetTexture.width, _cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, _cam.targetTexture.width, _cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        temp.SetActive(false); //Because C# garbage collection is garbage and only does it at end of frame >:(
        Destroy(temp);

        return Image;
    }
}
