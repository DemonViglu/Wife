using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DemonViglu.MouseInput;
public class Idle_State :IState
{
    private FSM manager;
    private Parameter parameter;

    private float timeToBegin;
    public Idle_State(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    void IState.OnEnter()
    {
        Debug.Log("Idle_Enter");
        InitialButtonEvent();
        timeToBegin = parameter.TimeGapFromIdle;
    }

    void IState.OnUpdate()
    {
        parameter.text.text = "Idle";
        AutoChat();
        HurtWife();
    }

    void IState.OnExit()
    {
        DeButtonEvent();
        Debug.Log("Idle_Exit");
    }

    public void AutoChat()
    {
        if (timeToBegin > 0)
        {
            timeToBegin -= Time.deltaTime;
            if (DialogSystemManager.instance.IsOnMission()) timeToBegin = parameter.TimeGapFromIdle;
            return;
        }
        else if (!DialogSystemManager.instance.IsOnMission() && timeToBegin < -100)
        {
            timeToBegin = parameter.TimeGapFromIdle+Random.Range(-parameter.TimeGapRandomRange/2,parameter.TimeGapRandomRange/2);
            return;
        }
        else if (timeToBegin < -100) return;

        if (parameter.emotionManager.GetEmotionXP() <= 5)
        {
            DialogSystemManager.instance.AddMissionSO(5);
        }
        else if (parameter.emotionManager.GetEmotionXP()>=80)
        {
            DialogSystemManager.instance.AddMissionSO(6);
        }
        else
        {
            DialogSystemManager.instance.AddMissionSO(0);
        }
        timeToBegin = -200;
    }

    void HurtWife()
    {
        if(Input.GetMouseButtonDown(0)&&MouseInputManager.instance.clickCounts>=3)
        {
            if (DialogSystemManager.instance.IsOnMission()) return;
            MouseInputManager.instance.ResetClickCount();
            parameter.emotionManager.ChangeEmotionXP(-20);
            DialogSystemManager.instance.AddMissionSO(4);
        }
    }


    private void InitialButtonEvent() {
        parameter.menuControllor.buttons[0].onClick.AddListener(ToPlay);
        parameter.menuControllor.buttons[1].onClick.AddListener(ToLove);
        parameter.menuControllor.buttons[3].onClick.AddListener(ToChat);
    }

    private void DeButtonEvent() {
        parameter.menuControllor.buttons[0].onClick.RemoveListener(ToPlay);
        parameter.menuControllor.buttons[1].onClick.RemoveListener(ToLove);
        parameter.menuControllor.buttons[3].onClick.RemoveListener(ToChat);
    }

    private void ToPlay() {
        if (manager.currentStateType != StateType.Idle) return;
        if (DialogSystemManager.instance.IsOnMission()) { return; }
        manager.TransitionState(StateType.Playing);
    }

    private void ToLove() {
        if (manager.currentStateType != StateType.Idle) return;
        if (DialogSystemManager.instance.IsOnMission()) { return; }
        manager.TransitionState(StateType.Love);
    }

    private void ToChat() {
        if (manager.currentStateType != StateType.Idle) return;
        if (DialogSystemManager.instance.IsOnMission()) { return; }
        manager.TransitionState(StateType.Chat);
    }


}
