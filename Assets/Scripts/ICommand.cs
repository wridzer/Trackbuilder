using System.Collections;
using UnityEngine;

public interface ICommand
{
    public GameObject Prefab { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public abstract void Execute();
    public abstract void Undo();
}