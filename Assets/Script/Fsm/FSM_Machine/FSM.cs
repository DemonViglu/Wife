using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.ComponentModel;

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
    public MenuControllor menuControllor;

    public float TimeGapFromIdle;
    public float TimeGapRandomRange;
}


public class FSM : MonoBehaviour
{
    public Parameter parameter;
    [ReadOnly]
    public StateType currentStateType;
    private IState currentState;
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
    }

    public void TransitionState(StateType state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[state];
        currentStateType = state;
        currentState.OnEnter();
    }






    
}
