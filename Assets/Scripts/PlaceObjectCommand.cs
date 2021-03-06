using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaceObjectCommand : ICommand
{
    public bool isErase { get; set; }
    public GameObject Prefab { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public GameObject GameObjectInstance { get; private set; }


    public PlaceObjectCommand(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Prefab = prefab;
        Position = position;
        Rotation = rotation;
        isErase = false;
    }
    public Dictionary<Vector3, GameObject> Execute(Dictionary<Vector3, GameObject> _gridDictionary)
    {
        GameObjectInstance = GameObject.Instantiate(Prefab, Position, Rotation);
        _gridDictionary[Position] = GameObjectInstance;
        return _gridDictionary;
    }

    public Dictionary<Vector3, GameObject> Undo(Dictionary<Vector3, GameObject> _gridDictionary)
    {
        Object.Destroy(GameObjectInstance);
        _gridDictionary[Position] = null;
        return _gridDictionary;
    }
}