using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Chat_State : IState
{
    private FSM manager;
    private Parameter parameter;

    private float timeToBegin;
    public Chat_State(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        Debug.Log("Chat Enter");
        parameter.ChatComponent.SetActive(true);
    }


    public void OnExit()
    {
        parameter.ChatComponent.SetActive(false);
        Debug.Log("Chat Exit");
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            manager.TransitionState(StateType.Idle);
        }
    }
}
