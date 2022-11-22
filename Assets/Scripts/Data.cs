using System.Collections;
using UnityEngine;

[System.Serializable]
public class Data
{
    public bool isErase { get; set; } // For importer to know if it's a Place or Erase command
    public float positionX { get; set; }
    public float positionY { get; set; }
    public float positionZ { get; set; }
    public float rotationX { get; set; }
    public float rotationY { get; set; }
    public float rotationZ { get; set; }
    public float rotationW { get; set; }
    public string PrefabName { get; set; }

}