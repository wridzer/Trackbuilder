using System.Collections;
using System;
using UnityEngine;


public class ToolState : State
{
    Type commandType { get; set; }

    Controls controller { get; set; }


    public ToolState(Type _commandType, GameObject _prefab, Controls _controller)
    {
        commandType = _commandType;
        prefab = _prefab;
        controller = _controller;
    }

    public override void OnEnter()
    {
        controller.toolPrefab = prefab;
    }

    public override void OnExit()
    {
        controller.eraseMode = false;
    }

    public override void OnUpdate()
    {
    }
}