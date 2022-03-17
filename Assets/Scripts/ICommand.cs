using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public abstract bool isErase { get; set; }
    public GameObject Prefab { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public abstract Dictionary<Vector3, GameObject> Execute(Dictionary<Vector3, GameObject> _gridDictionary);
    public abstract Dictionary<Vector3, GameObject> Undo(Dictionary<Vector3, GameObject> _gridDictionary);
}