using System;
using System.Collections;
using UnityEngine;


public abstract class State
{
    public GameObject prefab { get; set; }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}