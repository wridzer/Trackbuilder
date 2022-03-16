using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //ControlVariables
    private int scrollIndex = 0;
    private Vector3Int selectedPos;
    private GameObject selectPlane;

    //GridVariables
    [HideInInspector] public Vector3Int gridScale;
    [HideInInspector] public Dictionary<Vector3Int, GameObject> gridTiles = new Dictionary<Vector3Int, GameObject>();

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

    private void PlaceBlock()
    {
        //Remove old history
        for (int i = commandIndex + 1; i < commands.Count; i++)
        {
            commands.Remove(commands[i]);
        }

        //TODO: add rotation
        PlaceObjectCommand command = new PlaceObjectCommand(testPrefab, selectedPos, new Quaternion());
        commands.Add(command);
        command.Execute();
        commandIndex++;
    }

    private void Redo()
    {
        if(commandIndex < commands.Count - 1)
        {
            commandIndex++;
            commands[commandIndex].Execute();
        }
    }

    private void Undo()
    {
        if(commandIndex > -1)
        {
            commands[commandIndex].Undo();
            commandIndex--;
        }
    }

    private void Export()
    {
        Exporter.Export("testfile.text", commands);
    }

    private void Import()
    {
        commands = Importer.Import("testfile.text");
        foreach(ICommand command in commands)
        {
            command.Execute();
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
            Debug.DrawLine(ray.origin, hit.point);
            selectedPos = new Vector3Int(
                (int)hit.point.x,
                (int)hit.point.y,
                (int)hit.point.z);
        }
    }
}