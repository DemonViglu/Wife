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
        DialogManager.instance.BreakThrough();
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
                //if (DialogManager.instance.isOnPlay) return;
                DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 1);
                parameter.emotionManager.ChangeEmotionXP(10);
                parameter.playerImageManager.ChangeImage(ImageTpye.wow);
            }
            else
            {
                //ÎØÎØÎØ
                DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 2);
                parameter.playerImageManager.ChangeImage(ImageTpye.happy);
            }
            parameter.ball.ResetGame();
        }
        if (DialogManager.instance.JustFinishedPlay() && parameter.ball.HasPlayed())
        {
            parameter.ball.CloseScoreTap();
            manager.TransitionState(StateType.Idle);
            parameter.playerImageManager.ChangeImage(ImageTpye.what);
        }
        //when you won the game, changing the player's state and change tne play mode;
    }

    void IState.OnExit()
    {
        Debug.Log("Playing_Exit");
    }
}
