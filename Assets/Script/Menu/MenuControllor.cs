using DemonViglu.MouseInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllor : MonoBehaviour
{
    public GameObject menu;
    public Text text;
    public List<Button> buttons;
    void Update()
    {
        OpenAndClose();
    }

    public void OpenAndClose()
    {
        RaycastHit2D hitInfo = MouseInputManager.instance.mouseHitInfo;
        if (hitInfo.collider == null) return;
        else if(Input .GetMouseButtonDown(1)&&hitInfo.collider.transform.CompareTag("Player")&&GameManager.instance.playerManager.currentStateType==StateType.Idle)
        {
            if (DialogSystemManager.instance.IsOnMission()) return;
            menu.SetActive(!menu.activeSelf);
        }
    }
}
