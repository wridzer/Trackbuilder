using System.Collections;
using System;
using UnityEngine;


public class ToolState : State
{
    Type commandType { get; set; }

    Controls controller { get; set; }

    private bool isEraseTool;

    public ToolState(Type _commandType, GameObject _prefab, Controls _controller)
    {
        commandType = _commandType;
        prefab = _prefab;
        controller = _controller;

        if (commandType == typeof(EraseObjectCommand))
        {
            isEraseTool = true;
        }
    }

    public override void OnEnter()
    {
        if(isEraseTool)
        {
            controller.eraseMode = true;
        }
        else
        {
            controller.toolPrefab = prefab;
        }
    }

    public override void OnExit()
    {
        controller.eraseMode = false;
    }

    public override void OnUpdate()
    {
    }
}