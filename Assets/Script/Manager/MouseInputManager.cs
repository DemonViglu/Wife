using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseInputManager : MonoBehaviour
{
    public static MouseInputManager instance;
//      Instruction:
//      maxToCombo，就是单次点击最多耗时，如果你单次点击超过这个数值，就会判定点击失效（还不算长按）
//      minTimeHold，就是如果要达到所需的长按标准，需要多长时间，这是上一条的补充，但我没有写成互补，觉得好一点。
//      timeHolding就是当前鼠标持续按下了多久，当鼠标松开时计时清零
//      clickCount是在combo允许时间下连击次数。并且内置允许多次连击，所以当连击成功时，允许时间会恢复到maxTimeCombo
//      三个布尔值就很好理解了，一个自带的公开bool判断。
//      当然如果你有特殊的长按设置，比如当超过十秒，可以直接获取公开的timeHolding去判断。
    [Header("公开参数")]
    public float maxtimeToMousecombo;
    public float mintimeToMousehold;
    public float timeHolding;
    public int clickCounts;
    public bool isOneClick;
    public bool isMultiClick;
    public bool isHold;

    //[Header("内置参数")]
    private bool mouseDown;
    private bool mouseUp;
    private float tmpHolding;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    private void Start()
    {
        DataInit();
    }

    private void Update()
    {
        MouseStateUpdating();
        MouseHolding();
        MouseClick();
    }

    #region 数据初始化
    private void DataInit()
    {
        tmpHolding = 0;
        mouseUp = true;
        mouseDown = false;
    }
    #endregion

    #region 更新鼠标的当前状态和一些数值
    private void MouseStateUpdating()
    {
        //点击操作
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
            mouseUp = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseUp = true;
            mouseDown = false;
        }
        //内部点击次数数据与状态处理
        if (clickCounts == 1)
        {
            isOneClick = true;
            isMultiClick = false;
        }
        else if (clickCounts >= 2)
        {
            isOneClick = false;
            isMultiClick = true;
        }
        else if (clickCounts <= 0)
        {
            isOneClick= false;
            isMultiClick= false;
        }

        if (timeHolding >= mintimeToMousehold)
        {
            isHold = true;  
            isOneClick=false;
            isMultiClick=false;
        }
        else
        {
            isHold = false;
        }
    }
    #endregion

    #region 鼠标长按时间计时
    private void MouseHolding()
    {
        if (mouseDown)
        {
            timeHolding += Time.deltaTime;
        }
        else if(mouseUp)
        {
            timeHolding = 0;
        }
    }
    #endregion

    #region 当鼠标点击时，更新状态
    private void MouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (clickCounts == 0)
            {
                tmpHolding = maxtimeToMousecombo;
                clickCounts += 1;
            }
            else if (clickCounts >= 1)
            {
                if (0 < tmpHolding && tmpHolding < maxtimeToMousecombo)
                {
                    clickCounts += 1;
                    tmpHolding = maxtimeToMousecombo;
                }
            }
        }
        else
        {
            if (tmpHolding > 0)
            {
                tmpHolding-=Time.deltaTime;
            }
            else
            {
                tmpHolding=0;
                clickCounts = 0;
            }
        }
    }
    #endregion

    public void ResetClickCount()
    {
        clickCounts = 0;
    }
    public Collider2D GetTouchCollider2D()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.blue, duration: 1.0f);
        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.zero);
        //Debug.Log(hitInfo.collider);
        return hitInfo.collider;
    }
}
