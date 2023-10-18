using UnityEngine;
using System.Runtime.InteropServices; // 为了使用DllImport
using System;
/// <summary>
/// 让程序背景透明
/// </summary>
public class BackGroundSet : MonoBehaviour
{
    private IntPtr hwnd;

    private int currentX;

    private int currentY;

    #region Win函数常量
    private struct MARGINS

    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    [DllImport("user32.dll")]

    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]

    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]

    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]

    static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    [DllImport("Dwmapi.dll")]

    static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]

    private static extern uint SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);
    // 定义窗体样式,-16表示设定一个新的窗口风格

    private const int GWL_STYLE = -16;

    //设定一个新的扩展风格

    private const int GWL_EXSTYLE = -20;



    private const int WS_EX_LAYERED = 0x00080000;

    private const int WS_BORDER = 0x00800000;

    private const int WS_CAPTION = 0x00C00000;

    private const int SWP_SHOWWINDOW = 0x0040;

    private const int LWA_COLORKEY = 0x00000001;

    private const int LWA_ALPHA = 0x00000002;

    private const int WS_EX_TRANSPARENT = 0x20;

    #endregion



    void Awake()

    {

        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        var productName = Application.productName;

#if !UNITY_EDITOR

        // 获得窗口句柄
        hwnd = FindWindow(null, productName); 
        // 设置窗体属性
        int intExTemp = GetWindowLong(hwnd, GWL_EXSTYLE); // 获得当前样式
        SetWindowLong(hwnd, GWL_EXSTYLE, intExTemp | WS_EX_LAYERED); // 当前样式加上WS_EX_LAYERED     // WS_EX_TRANSPARENT 收不到点击的透明
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION); // 无边框、无标题栏
        // 设置窗体位置为右下角

        currentX = Screen.currentResolution.width - 900;
        currentY = Screen.currentResolution.height  - 800;

        SetWindowPos(hwnd, -1, currentX, currentY, 1200, 900, SWP_SHOWWINDOW); // Screen.currentResolution.width 
        // 扩展窗口到客户端区域 -> 为了透明
        var margins = new MARGINS() { cxLeftWidth = -1 }; // 边距内嵌值确定在窗口四侧扩展框架的距离 -1为没有窗口边框
        DwmExtendFrameIntoClientArea(hwnd, ref margins);     

        // 将该窗口颜色为0的部分设置为透明,即背景可穿透点击，人物模型上不穿透

        SetLayeredWindowAttributes(hwnd, 0, 255, 1);

        /// <summary>

        ///设置窗体可穿透点击的透明.

        ///参数1:窗体句柄

        ///参数2:透明颜色  0为黑色,按照从000000到FFFFFF的颜色,转换为10进制的值

        ///参数3:透明度,设置成255就是全透明

        ///参数4:透明方式,1表示将该窗口颜色为0的部分设置为透明,2表示根据透明度设置窗体的透明度

        /// </summary>



#endif



        /// <summary>

        /// 1

        /// 调节窗体透明度可以先使用SetWindowLong为窗体加上WS_EX_LAYERED属性，

        /// 再使用SetLayeredWindowAttributes来指定窗体的透明度。

        /// 这样就可以在程序运行时动态的调节窗体的透明度了。

        /// 2

        /// 给 GWL_EXSTYLE 设置 WS_EX_TRANSPARENT 让窗口透明，此时应用程序只能收到鼠标消息但收不到触摸消息

        /// 3

        /// 前面加上取反操作符"~"，就可以得到相反效果。比如，WS_CAPTION代表窗口有标题栏，~WS_CAPTION代表窗口没有标题栏

        /// 4

        /// GWL_STYLE指的是那些旧的窗口属性。相对于GWL_EXSTYLEGWL扩展属性而言的

        /// 5

        /// 要给窗口添加某属性，用 | 来连接，要去除某属性，用 & 来连接

        /// </summary>

    }

}
//作者：uni_shiki  出处：bilibili