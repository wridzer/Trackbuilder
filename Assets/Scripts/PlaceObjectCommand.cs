using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlaceObjectCommand : ICommand
{
    public GameObject Prefab { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public GameObject GameObjectInstance { get; private set; }


    public PlaceObjectCommand(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Prefab = prefab;
        Position = position;
        Rotation = rotation;
    }
    public void Execute()
    {
        GameObjectInstance = GameObject.Instantiate(Prefab, Position, Rotation);
    }

    public void Undo()
    {
        Object.Destroy(GameObjectInstance);
    }
}