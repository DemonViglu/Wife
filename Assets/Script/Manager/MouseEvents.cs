//using System;

//using System.Collections;

//using System.Collections.Generic;

//using System.Runtime.InteropServices;

//using UnityEngine;

//using UnityEngine.EventSystems;



//public class MouseEvents : MonoBehaviour

//{

//    [DllImport("user32.dll")]

//    public static extern short GetAsyncKeyState(int vKey);

//    private const int VK_LBUTTON = 0x01; //鼠标左键

//    private const int VK_RBUTTON = 0x02; //鼠标右键

//    private const int VK_MBUTTON = 0x04; //鼠标中键



//    private bool _isLeftDown;

//    private bool _isRightDown;

//    private bool _isMiddleDown;



//    public event Action<MouseKey, Vector3> MouseKeyDownEvent;

//    public event Action<MouseKey, Vector3> MouseKeyUpEvent;

//    public event Action<MouseKey, Vector3> MouseDragEvent;

//    public event Action<MouseKey> MouseKeyClickEvent;



//    public Vector3 MousePos { get; private set; }



//    private bool _hasDragged;

//    private Vector3 _leftDownPos;

//    private Vector3 _rightDownPos;

//    private Vector3 _middleDownPos;



//    [SerializeField] private Animator anim;



//    [DllImport("user32.dll")]

//    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

//    /// <summary>

//    /// 获得鼠标在屏幕上的位置

//    /// </summary>

//    /// <param name="lpPoint"></param>

//    /// <returns></returns>

//    [DllImport("user32.dll")]

//    static extern bool GetCursorPos(ref POINT lpPoint);





//    /// <summary>

//    /// 记录当前鼠标的位置

//    /// </summary>

//    public POINT point;



//    /// <summary>

//    /// 设置目标窗体大小，位置

//    /// </summary>

//    /// <param name="hWnd">目标句柄</param>

//    /// <param name="x">目标窗体新位置X轴坐标</param>

//    /// <param name="y">目标窗体新位置Y轴坐标</param>

//    /// <param name="nWidth">目标窗体新宽度</param>

//    /// <param name="nHeight">目标窗体新高度</param>

//    /// <param name="BRePaint">是否刷新窗体</param>

//    /// <returns></returns>

//    [DllImport("user32.dll", CharSet = CharSet.Auto)]

//    public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

//    /// <summary>

//    /// 当前窗体句柄

//    /// </summary>

//    private IntPtr hwnd;



//    private bool dragModle; // 是否在拖拽模型

//    [SerializeField] private Transform neck; // 脖子部位，控制模型朝向鼠标位置的开关



//    [Header("右键主菜单")]

//    [SerializeField] private GameObject mainMenu;

//    [Header("设置大小页")]

//    [SerializeField] private GameObject sizePage;



//    private void Start()

//    {

//        Init();

//    }



//    // 失焦时关闭主菜单

//    private void OnApplicationFocus(bool focus)

//    {

//        if (!focus && mainMenu.activeSelf)

//        {

//            mainMenu.SetActive(false);

//        }

//    }



//    private void Update()

//    {

//        // 按下左键

//        if (GetAsyncKeyState(VK_LBUTTON) != 0)

//        {

//            // 左键不在UI上，关闭主菜单

//            if (mainMenu.activeSelf && !EventSystem.current.IsPointerOverGameObject())

//            {

//                mainMenu.SetActive(false);

//            }



//            if (!_isLeftDown)

//            {

//                _isLeftDown = true;

//                _leftDownPos = MouseKeyDown(MouseKey.Left);

//            }

//            else if (MousePos != Input.mousePosition)

//            {

//                MouseKeyDrag(MouseKey.Left);

//                if (!_hasDragged)

//                {

//                    _hasDragged = true;



//                    // 开启拖拽模型，根据鼠标位置判断是否可拖拽。显示模型时用碰撞体+射线检测，在模型不显示时可用是否在UI上判断

//                    if (Physics.Raycast(Camera.main.ScreenPointToRay(MousePos), out RaycastHit hit, 10f, 1 << LayerMask.NameToLayer("Player")) || (EventSystem.current.IsPointerOverGameObject() && (UIScript.Instance.inGameMode || UIScript.Instance.inWorkMode)))

//                    {

//                        dragModle = true;

//                        mainMenu.SetActive(false);

//                    }

//                }

//            }

//        }

//        // 按下右键

//        if (GetAsyncKeyState(VK_RBUTTON) != 0)

//        {

//            sizePage.SetActive(false);

//            // 右键模型出现主菜单,并根据鼠标位置设置主菜单出现位置，防止主菜单显示到屏幕外，是把屏幕分为四个象限来判断

//            if (Physics.Raycast(Camera.main.ScreenPointToRay(MousePos), out RaycastHit hit, 10f, 1 << LayerMask.NameToLayer("Player")) || (EventSystem.current.IsPointerOverGameObject() && (UIScript.Instance.inGameMode || UIScript.Instance.inWorkMode)))

//            {

//                if (point.X <= Screen.currentResolution.width / 2 && point.Y <= Screen.currentResolution.height / 2)

//                {

//                    mainMenu.transform.localPosition = new Vector3(Input.mousePosition.x * 3 - 1500, Input.mousePosition.y * 3 - 1800, 0);

//                }

//                else if (point.X > Screen.currentResolution.width / 2 && point.Y <= Screen.currentResolution.height / 2)

//                {

//                    mainMenu.transform.localPosition = new Vector3(Input.mousePosition.x * 3 - 1500 - 600, Input.mousePosition.y * 3 - 1800, 0);

//                }

//                else if (point.X <= Screen.currentResolution.width / 2 && point.Y > Screen.currentResolution.height / 2)

//                {

//                    mainMenu.transform.localPosition = new Vector3(Input.mousePosition.x * 3 - 1500, Input.mousePosition.y * 3 - 1800 + 800, 0);

//                }

//                else if (point.X > Screen.currentResolution.width / 2 && point.Y > Screen.currentResolution.height / 2)

//                {

//                    mainMenu.transform.localPosition = new Vector3(Input.mousePosition.x * 3 - 1500 - 600, Input.mousePosition.y * 3 - 1800 + 800, 0);

//                }

//                mainMenu.SetActive(true);

//            }





//            if (!_isRightDown)

//            {

//                _isRightDown = true;

//                _rightDownPos = MouseKeyDown(MouseKey.Right);

//            }

//            else if (MousePos != Input.mousePosition)

//            {

//                MouseKeyDrag(MouseKey.Right);

//                if (!_hasDragged)

//                {

//                    _hasDragged = true;

//                }

//            }

//        }

//        // 按下中键

//        if (GetAsyncKeyState(VK_MBUTTON) != 0)

//        {

//            if (!_isMiddleDown)

//            {

//                _isMiddleDown = true;

//                _middleDownPos = MouseKeyDown(MouseKey.Middle);

//            }

//            else if (MousePos != Input.mousePosition)

//            {

//                MouseKeyDrag(MouseKey.Middle);

//                if (!_hasDragged)

//                {

//                    _hasDragged = true;

//                }

//            }

//        }

//        // 抬起左键

//        if (GetAsyncKeyState(VK_LBUTTON) == 0 && _isLeftDown)

//        {

//            _isLeftDown = false;

//            MouseKeyUp(MouseKey.Left);



//            // 无拖拽、downPos==upPos

//            if (!_hasDragged && _leftDownPos == MousePos)

//            {

//                MouseKeyClick(MouseKey.Left);



//                // 点击模型的触发动作设置

//                if (!anim.GetBool("bow") && !anim.GetBool("exit") && !UIScript.Instance.randomAnimMode && !UIScript.Instance.inGameMode && Physics.Raycast(Camera.main.ScreenPointToRay(MousePos), out RaycastHit hit, 10f, 1 << LayerMask.NameToLayer("Player")))

//                {

//                    int num = UnityEngine.Random.Range(1, 3); // 1/2

//                    neck.transform.rotation = Quaternion.Euler(0, 0, 0);

//                    switch (num)

//                    {

//                        case 1:

//                            GameObject.FindWithTag("Player").GetComponent<Animator>().SetBool("bow", true); break;

//                        case 2:

//                            GameObject.FindWithTag("Player").GetComponent<Animator>().SetBool("exit", true); break;

//                    }

//                }







//            }



//            _hasDragged = false;

//            // 停止拖拽模型

//            dragModle = false;



//        }

//        // 抬起右键

//        if (GetAsyncKeyState(VK_RBUTTON) == 0 && _isRightDown)

//        {

//            _isRightDown = false;

//            MouseKeyUp(MouseKey.Right);



//            if (!_hasDragged && _rightDownPos == MousePos)

//            {

//                MouseKeyClick(MouseKey.Right);

//            }



//            _hasDragged = false;

//        }

//        // 抬起中键

//        if (GetAsyncKeyState(VK_MBUTTON) == 0 && _isMiddleDown)

//        {

//            _isMiddleDown = false;

//            MouseKeyUp(MouseKey.Middle);



//            if (!_hasDragged && _middleDownPos == MousePos)

//            {

//                MouseKeyClick(MouseKey.Middle);

//            }



//            _hasDragged = false;

//        }





//        // 拖拽

//        if (dragModle)

//        {

//            GetCursorPos(ref point); // 获取鼠标在屏幕上的位置（原点在左上）.而不是鼠标在unity中的位置（原点在左下）

//            MoveWindow(hwnd, point.X - Screen.width / 2, point.Y - Screen.height / 2, Screen.width, Screen.height, true);

//        }

//    }



//    public void Init()

//    {

//        _isLeftDown = false;

//        _isRightDown = false;

//        _isMiddleDown = false;

//        _hasDragged = false;

//        hwnd = FindWindow(null, Application.productName);

//        dragModle = false;



//        point.X = Screen.currentResolution.width;

//        point.Y = Screen.currentResolution.height;

//    }





//    private Vector3 MouseKeyDown(MouseKey mouseKey)

//    {

//        RefreshMousePos();

//        MouseKeyDownEvent?.Invoke(mouseKey, MousePos);



//        return MousePos;

//    }

//    private Vector3 MouseKeyUp(MouseKey mouseKey)

//    {

//        RefreshMousePos();

//        MouseKeyUpEvent?.Invoke(mouseKey, MousePos);



//        return MousePos;

//    }



//    private Vector3 MouseKeyDrag(MouseKey mouseKey)

//    {

//        RefreshMousePos();

//        MouseDragEvent?.Invoke(mouseKey, MousePos);



//        return MousePos;

//    }



//    private void MouseKeyClick(MouseKey mouseKey)

//    {

//        MouseKeyClickEvent?.Invoke(mouseKey);

//    }



//    private Vector3 RefreshMousePos()

//    {

//        MousePos = Input.mousePosition;

//        return MousePos;

//    }



//    public Vector3 MousePosToWorldPos(Vector3 mousePos)

//    {

//        var screenInWorldPos = Camera.main.WorldToScreenPoint(mousePos);

//        var refPos = new Vector3(mousePos.x, mousePos.y, screenInWorldPos.z);

//        var pos = Camera.main.ScreenToWorldPoint(refPos);

//        return pos;

//    }

//}



//public enum MouseKey

//{

//    None,

//    Left,

//    Right,

//    Middle

//}



//public struct POINT

//{

//    public int X;

//    public int Y;



//    public POINT(int x, int y)

//    {

//        this.X = x;

//        this.Y = y;

//    }



//    public override string ToString()

//    {

//        return ("X:" + X + ", Y:" + Y);

//    }

//}




////三、人物模型看向鼠标位置

////代码：


////using UnityEngine;



////public class TowardMousePos : MonoBehaviour

////{

////    [SerializeField] private Transform neck;

////    [SerializeField] private Animator anim;



////    private void Update()

////    {

////        // 设置分辨率为1200:900可以计算出鼠标当前位置（设置别的也行，自行调试）

////        float x = Mathf.Clamp((Input.mousePosition.x - 1200 * .5f) / Screen.width * .5f * 180, -50, 50);

////        float y = Mathf.Clamp((Input.mousePosition.y - 900 * .5f) / Screen.height * .5f * 180, -20, 50);

////        // 旋转脖子角度(可加if判断)

////        neck.transform.rotation = Quaternion.Euler(-y + 45, -x, 0); // 上下，左右



////    }

////}
////作者：uni_shiki https://www.bilibili.com/read/cv21407303/?spm_id_from=333.999.0.0 出处：bilibili