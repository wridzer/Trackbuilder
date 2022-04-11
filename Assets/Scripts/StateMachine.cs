using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMachine
{
    private State currentState;
    private Dictionary<GameObject, State> allStates = new Dictionary<GameObject, State>();

    public void AddState(State _State)
    {
        allStates.Add(_State.prefab, _State);
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }

    public void SwitchState(GameObject _Prefab)
    {
        currentState?.OnExit();
        if (allStates.ContainsKey(_Prefab))
        {
            currentState = allStates[_Prefab];
        }
        currentState?.OnEnter();
    }

    public void ClearStates()
    {
        allStates.Clear();
    }
}