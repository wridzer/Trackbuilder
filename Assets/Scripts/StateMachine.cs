using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMachine<T>
{
    private State<T> currentState;
    private Dictionary<System.Type, State<T>> allStates = new Dictionary<System.Type, State<T>>();

    public T pOwner
    {
        get;
        protected set;
    }

    public StateMachine(T _owner)
    {
        pOwner = _owner;
    }

    public void AddState(State<T> _State)
    {
        allStates.Add(_State.GetType(), _State);
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }

    public void SwitchState(System.Type _type)
    {
        currentState?.OnExit();
        if (allStates.ContainsKey(_type))
        {
            currentState = allStates[_type];
        }
        currentState?.OnEnter();
    }
}