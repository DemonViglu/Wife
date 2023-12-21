using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Playing_State : IState
{

    private FSM manager;
    private Parameter parameter;

    public Playing_State(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    void IState.OnEnter()
    {
        DialogSystemManager.instance.ClearMissionRightNow();
        DialogSystemManager.instance.missionEventHandler._OnEveryMissionEnd += OnBallGameEnd;
        Debug.Log("Playing_Enter");
        parameter.ball.BeginToShoot();
        GameManager.instance.Shooting();

        parameter.playerImageManager.ChangeImage(ImageTpye.what);
    }

    void IState.OnUpdate()
    {
        parameter.text.text = "Playing";

        if (parameter.ball.finished)
        {
            if (parameter.ball.score >= 9)
            {
                //ÄãÕæ°ô°ô
                DialogSystemManager.instance.AddMissionSO(1);
                parameter.emotionManager.ChangeEmotionXP(10);
                parameter.playerImageManager.ChangeImage(ImageTpye.wow);
            }
            else
            {
                //ÎØÎØÎØ
                DialogSystemManager.instance.AddMissionSO(2);
                parameter.playerImageManager.ChangeImage(ImageTpye.happy);
            }
            parameter.ball.ResetGame();
        }
    }

    void IState.OnExit()
    {
        DialogSystemManager.instance.missionEventHandler._OnEveryMissionEnd -= OnBallGameEnd;
        Debug.Log("Playing_Exit");
    }
    /// <summary>
    /// this Ending is base on the dialog finished;
    /// </summary>
    /// <param name="a"></param>
    private void OnBallGameEnd(int a) {
        if(a==1||a==2) {
            parameter.ball.CloseScoreTap();
            manager.TransitionState(StateType.Idle);
            parameter.playerImageManager.ChangeImage(ImageTpye.what);
        }
    }
}
