using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlaceObjectCommand : ICommand
{
    public GameObject Prefab { get; private set; }
    public Vector3 Position { get; private set; }
    public Quaternion Rotation { get; private set; }
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