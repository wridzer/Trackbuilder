using System.Collections;
using UnityEngine;

public class ToolButton : MonoBehaviour
{
    private StateMachine machine;
    private State state;

    public void SetVariables(StateMachine _stateMachine, State _state)
    {
        machine = _stateMachine;
        state = _state;
    }

    public void OnButtonPressed()
    {
        machine?.SwitchState(state.prefab);
    }
}