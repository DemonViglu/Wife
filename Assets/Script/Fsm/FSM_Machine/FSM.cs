using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum StateType
{ 
    Idle, Playing,Love,Chat
}

[Serializable]
public class Parameter
{
    public Text text;
    public BallGameManager ball;
    public EmotionUIManageer emotionManager;
    public PlayerImageManager playerImageManager;
    public GameObject ChatComponent;

    public float TimeGapFromIdle;
}


public class FSM : MonoBehaviour
{
    public Parameter parameter;
    public IState currentState;
    private Dictionary<StateType,IState> states=new Dictionary<StateType,IState>();
    void Start()
    {
        states.Add(StateType.Idle, new Idle_State(this));
        states.Add(StateType.Playing, new Playing_State(this));
        states.Add(StateType.Love, new Love_State(this));
        states.Add(StateType.Chat, new Chat_State(this));

        TransitionState(StateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
        //TextToDebug();
    }

    public void TransitionState(StateType state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[state];
        currentState.OnEnter();
    }
    private void TextToDebug()
    {
        parameter.text.text=Input.mousePosition.x.ToString()+"    "+Input.mousePosition.y.ToString();
    }
}
