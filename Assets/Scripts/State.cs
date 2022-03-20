using System;
using System.Collections;
using UnityEngine;


public abstract class State<T>
{
    Type commandType { get; set; }

    GameObject prefab { get; set; }

    State(Type _commandType, GameObject _prefab)
    {
        commandType = _commandType;
        prefab = _prefab;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}