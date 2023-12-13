using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        GameManager.instance.Chat();

        timeToBegin =(float) parameter.dialogSetting.TheTimeToChat();
    }

    void IState.OnUpdate()
    {
        parameter.text.text = "Idle";

        if(GameManager.instance.currentGame==PlayMode.Shooting)
        {
            manager.TransitionState(StateType.Playing);
            //GameManager.instance.currentGame = PlayMode.none;
        }
        if (GameManager.instance.currentGame == PlayMode.Gifting)
        {
            manager.TransitionState(StateType.Love);
        }
        if (GameManager.instance.currentGame == PlayMode.ChatWithGPT)
        {
            manager.TransitionState(StateType.Chat);
        }
        AutoChat();
        HurtWife();
    }

    void IState.OnExit()
    {
        Debug.Log("Idle_Exit");
    }

    public void AutoChat()
    {
        if (timeToBegin > 0)
        {
            timeToBegin -= Time.deltaTime;
            if (DialogManager.instance.isBusy) timeToBegin = parameter.dialogSetting.TheTimeToChat();
            return;
        }
        else if (!DialogManager.instance.isBusy && timeToBegin < -100)
        {
            timeToBegin = parameter.dialogSetting.TheTimeToChat();
            return;
        }
        else if (timeToBegin < -100) return;

        if (parameter.emotionManager.GetEmotionXP() <= 5)
        {
            //DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 5);
            DialogSystemManager.instance.AddMissionSO(5);
        }
        else if (parameter.emotionManager.GetEmotionXP()>=80)
        {
            //DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 6);
            DialogSystemManager.instance.AddMissionSO(6);
        }
        else
        {
            //DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 0);
            DialogSystemManager.instance.AddMissionSO(0);
        }
        timeToBegin = -200;
    }

    void HurtWife()
    {
        if(Input.GetMouseButtonDown(0)&&MouseInputManager.instance.clickCounts>=3)
        {
            if (DialogManager.instance.isOnPlay) return;
            MouseInputManager.instance.ResetClickCount();
            parameter.emotionManager.ChangeEmotionXP(-20);
            //DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 4);
            DialogSystemManager.instance.AddMissionSO(4);
        }
    }
}
