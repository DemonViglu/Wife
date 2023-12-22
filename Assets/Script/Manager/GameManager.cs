using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public FSM playerManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        WindowSetInit();
    }

    private void Update()
    {
        DragWife();
    }



    private bool isDrag;
    public Transform playerTrans;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lpPoint"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(ref POINT lpPoint);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);
    [DllImport("user32.dll")]

    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public override string ToString()
        {
            return ("X:" + X + ", Y:" + Y);
        }
    }
    private IntPtr hwnd;
    public POINT point;
    public void DragWife()
    {

        if (isDrag&&playerManager.currentStateType==StateType.Idle)
        {
            //playerTrans.position =new Vector2( Camera.main.ScreenPointToRay(Input.mousePosition).origin.x, Camera.main.ScreenPointToRay(Input.mousePosition).origin.y);
            GetCursorPos(ref point); // 获取鼠标在屏幕上的位置（原点在左上）.而不是鼠标在unity中的位置（原点在左下）

            MoveWindow(hwnd, point.X - Screen.width / 2, point.Y - Screen.height / 2, Screen.width, Screen.height, true);

        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 20f, UnityEngine.Color.blue, duration: 1.0f);
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.zero);
        if (hitInfo.collider == null) return;
        else if (hitInfo.collider.transform.CompareTag("Player")&&Input.GetMouseButtonDown(0))
        {
            isDrag = true;
        }
        //Debug.Log(hitInfo.collider);
        if(Input.GetMouseButtonUp(0)) { isDrag = false; }
    }


    void WindowSetInit()
    {
        hwnd = FindWindow(null, Application.productName);
        point.X = Screen.currentResolution.width;
        point.Y = Screen.currentResolution.height;
    }
}
