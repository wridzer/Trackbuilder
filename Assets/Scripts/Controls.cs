using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls
{
    //ControlVariables
    private int scrollIndex = 0;
    private Vector3Int selectedPos;

    //GridVariables
    private int gridHeight;
    private Dictionary<Vector3Int, GameObject> gridTiles = new Dictionary<Vector3Int, GameObject>();

    //CommandPattern
    private List<ICommand> commands = new List<ICommand>();
    private int commandIndex = -1;

    //DebugVariables
    public GameObject testPrefab;

    public Controls(Dictionary<Vector3Int, GameObject> _gridTiles, int _gridHeight, GameObject _testPrefab)
    {
        gridHeight = _gridHeight;
        gridTiles = _gridTiles;
        testPrefab = _testPrefab;
    }

    public void Update()
    {
        HandleInput();
        SelectBlock();
        VisualizeRow();
    }

    private void VisualizeRow()
    {
        foreach (KeyValuePair<Vector3Int, GameObject> gridTile in gridTiles)
        {
            if (gridTile.Key == selectedPos)
            {
                gridTile.Value.GetComponentInChildren<BoxCollider>().enabled = true;
                gridTile.Value.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                gridTile.Value.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
            else if (gridTile.Key.y == scrollIndex)
            {
                gridTile.Value.GetComponentInChildren<BoxCollider>().enabled = true;
                gridTile.Value.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                gridTile.Value.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
            else
            {
                gridTile.Value.GetComponentInChildren<BoxCollider>().enabled = false;
                gridTile.Value.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    private void HandleInput()
    {
        //Select Height
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (scrollIndex < gridHeight - 1)
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

    //Get selected Place
    private void SelectBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point);
            if(hit.transform.parent != null)
            {
                selectedPos = new Vector3Int(
                    (int)hit.transform.parent.transform.position.x,
                    (int)hit.transform.parent.transform.position.y,
                    (int)hit.transform.parent.transform.position.z);
            } else
            {
                //TODO: make it invalid!!
                //place is invalid
                selectedPos = new Vector3Int(666, 666, 666);
            }
        }
    }
}