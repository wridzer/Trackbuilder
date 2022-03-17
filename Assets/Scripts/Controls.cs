using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //ControlVariables
    private int scrollIndex = 0;
    private Vector3Int selectedPos;
    private bool isSelectedPlaceValid;
    private GameObject selectPlane;
    private Vector3 objectRotation;

    //GridVariables
    [HideInInspector] public Vector3Int gridScale;
    [HideInInspector] public Dictionary<Vector3, GameObject> gridTiles = new Dictionary<Vector3, GameObject>();

    //CommandPattern
    private List<ICommand> commands = new List<ICommand>();
    private int commandIndex = -1;

    //DebugVariables
    public GameObject testPrefab;

    public void Start()
    {
        selectPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        selectPlane.GetComponent<MeshCollider>().convex = true;
        selectPlane.transform.localScale = new Vector3(gridScale.x, 10f, gridScale.z) * 0.1f;
        selectPlane.transform.position = new Vector3(gridScale.x * 0.5f - 0.5f, 0, gridScale.z * 0.5f - 0.5f);
    }

    public void Update()
    {
        HandleInput();
        SelectBlock();
        VisualizeRow();
    }

    private void VisualizeRow()
    {
        selectPlane.transform.position = new Vector3(selectPlane.transform.position.x, scrollIndex - 0.49f, selectPlane.transform.position.z);
    }

    private void HandleInput()
    {
        //Select Height
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (scrollIndex < gridScale.y - 1)
            {
                scrollIndex++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (scrollIndex > 0)
            {
                scrollIndex--;
            }
        }
        //Rotation
        if (Input.GetKeyDown(KeyCode.R))
        {
            objectRotation.x += 90f;
            if(objectRotation.x == 360) { objectRotation.x = 0; }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            objectRotation.y += 90f;
            if (objectRotation.x == 360) { objectRotation.x = 0; }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            objectRotation.z += 90f;
            if (objectRotation.x == 360) { objectRotation.x = 0; }
        }
        //Click Input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            PlaceBlock();
        }
        //Redo an Undo
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Undo();
        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Redo();
        }
        //Import and Export
        if (Input.GetKeyDown(KeyCode.E))
        {
            Export();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Import();
        }
    }

    private void EraseBlock()
    {
        RemoveHistory();
        
        if (!isSelectedPlaceValid)
        {
            EraseObjectCommand command = new EraseObjectCommand(
                gridTiles[selectedPos],
                gridTiles[selectedPos].transform.position,
                gridTiles[selectedPos].transform.rotation);
            commands.Add(command);
            gridTiles = command.Execute(gridTiles);
            commandIndex++;
        }
    }

    private void PlaceBlock()
    {
        RemoveHistory();

        //Translate rotation
        Quaternion placementRotation = Quaternion.Euler(objectRotation);

        //Check if place is valid and place block
        if (isSelectedPlaceValid)
        {
            PlaceObjectCommand command = new PlaceObjectCommand(testPrefab, selectedPos, placementRotation);
            commands.Add(command);
            gridTiles = command.Execute(gridTiles);
            commandIndex++;
        }
    }

    private void Redo()
    {
        if(commandIndex < commands.Count - 1)
        {
            commandIndex++;
            gridTiles = commands[commandIndex].Execute(gridTiles);
        }
    }

    private void Undo()
    {
        if(commandIndex > -1)
        {
            gridTiles = commands[commandIndex].Undo(gridTiles);
            commandIndex--;
        }
    }

    private void Export()
    {
        //Export only until current index
        List<ICommand> exportCommands = new List<ICommand>();
        for(int i = 0; i <= commandIndex; i++)
        {
            exportCommands.Add(commands[i]);
        }

        Exporter.Export("testfile.text", exportCommands);
    }

    private void Import()
    {
        //TODO: clear lists and dictionaries and remove old objects

        commands = Importer.Import("testfile.text");
        foreach(ICommand command in commands)
        {
            gridTiles = command.Execute(gridTiles);
        }
        commandIndex = commands.Count - 1;
    }

    //Get selected Place
    private void SelectBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            selectedPos = new Vector3Int(
                (int)hit.point.x,
                scrollIndex,
                (int)hit.point.z);
            if(gridTiles[selectedPos] == null) { isSelectedPlaceValid = true; } else { isSelectedPlaceValid = false; }
        }
    }

    private void RemoveHistory()
    {
        for (int i = commands.Count - 1; i > commandIndex; i--)
        {
            commands.Remove(commands[i]);
        }
    }
}