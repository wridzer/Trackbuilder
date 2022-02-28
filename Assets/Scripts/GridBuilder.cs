using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    //GridSettings
    [SerializeField] private int gridHeight, gridWidth, gridLength;
    private List<Vector3Int> gridPositions = new List<Vector3Int>();

    //ControlSettings
    private int scrollIndex = 0;
    private Camera cam;
    [SerializeField] private Vector3Int selectedPos;

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
        //set camera
        cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(scrollIndex < gridHeight - 1)
            {
                scrollIndex++;
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(scrollIndex > 0)
            {
                scrollIndex--;
            }
        }
    }

    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        float distanceGridToCam = Vector3.Distance(cam.transform.position, new Vector3(gridWidth / 2, scrollIndex, gridLength / 2));

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, distanceGridToCam + (mousePos.y * 0.001f)));

        selectedPos = new Vector3Int((int)point.x, scrollIndex, (int)point.z);

        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.EndArea();
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        foreach(Vector3Int gridPos in gridPositions)
        {
            if(gridPos == selectedPos)
            {
                Gizmos.color = Color.cyan;
            }
            else if(gridPos.y == scrollIndex)
            {
                Gizmos.color = Color.magenta;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawSphere(gridPos, 0.2f);
        }
    }
}
