using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Love_State : IState
{
    private FSM manager;
    private Parameter parameter;

    public Love_State(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    void IState.OnEnter()
    {
        GameManager.instance.Gifting();
        Debug.Log("Love_Enter");
    }

    void IState.OnUpdate()
    {
        Gift();
    }

    void IState.OnExit()
    {
        Debug.Log("Love_Exit");
        EnvironmentManager.instance.FlowerEnd();
    }

    void Gift()
    {
        if (Input.GetMouseButtonDown(0) && !DialogManager.instance.isOnPlay)
        {
            parameter.playerImageManager.ChangeImage(ImageTpye.shy);
            EnvironmentManager.instance.FlowerBegin();
            DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 3);
            parameter.emotionManager.ChangeEmotionXP(50);
            if (parameter.emotionManager.GetEmotionXP() >= 80)
            {
                DialogManager.instance.PreLoadTheFile(parameter.dialogSetting.autoPlayTime, 6);
            }
        }
        if (DialogManager.instance.JustFinishedPlay())
        {
            manager.TransitionState(StateType.Idle);
            parameter.playerImageManager.ChangeImage(ImageTpye.what);

        }
    }
}
