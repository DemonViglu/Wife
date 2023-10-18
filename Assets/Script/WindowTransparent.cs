using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class WindowTransparent : MonoBehaviour
{
    [DllImport("user32.dll")]

    public static extern int MessageBox(IntPtr hWnd, string msg,string caption,uint type);



    private struct MARGINS 
    {
        public int cxLeftWidth;
        public int cxTopHeight;
        public int cxRightWidth;
        public int cxBottomHeight;
    }

    [DllImport("user32.dll")]

    private static extern IntPtr GetActiveWindow();

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]

    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewlong);

    [DllImport("user32.dll", SetLastError=true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlag);

    [DllImport("user32.dll")]

    static extern int SetLayeredWindowAttributes(IntPtr hWnd,uint crKey,byte bAlpha,uint dwFlags);

    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const uint LWA_COLORKEY = 0x00000001;
    void Start()
    {
        //MessageBox(new IntPtr(0), "hello world", "shit", 0);
#if !UNITY_EDITOR
        IntPtr hWnd = GetActiveWindow();

        MARGINS margins = new MARGINS { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea (hWnd, ref margins);
        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);// | WS_EX_TRANSPARENT
        SetLayeredWindowAttributes(hWnd,0,0,LWA_COLORKEY);
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0,0);
#endif
        Application.runInBackground = true;
    }
}
