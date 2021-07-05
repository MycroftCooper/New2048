using UnityEngine;
using static GameController_Mode1;

public class UserInput_Mode1 : MonoBehaviour,UserInput
{
    private GameController_Mode1 GC;

    void Start()
        => GC = GameObject.Find("GameController").GetComponent<GameController_Mode1>();

    void Update()
    {
        PCInput();
        PhoneInput();
    }

    // 处理电脑的输入
    public void PCInput()
    {
        if (!GC.IsPause())
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                GC.Move_EventHandle(MoveDirection.Up);
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                GC.Move_EventHandle(MoveDirection.Down);
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
                GC.Move_EventHandle(MoveDirection.Left);
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                GC.Move_EventHandle(MoveDirection.Right);
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Escape))
            GC.GamePause(!GC.IsPause());
    }

    // 处理移动设备的输入
    Vector2 m_ptStart;//起点位置
    float m_TimeStart;//触摸开始时间
    bool m_Flag = false;
    int m_Len = 30; // 滑动的有效长度
    public void PhoneInput()
    {
        if ( Input.touchCount == 0 ) return;
        Touch touch = Input.GetTouch( 0 );
        // 按下
        if ( touch.phase == TouchPhase.Began )
        {  
            // 记录起点位置
            m_ptStart = touch.position;
            // 记录触摸开始时间
            m_TimeStart = Time.fixedTime;
            // 本次操作有效
            m_Flag = true;
        }
        // 滑动
        else if ( touch.phase == TouchPhase.Moved )
        {   
            if (m_Flag == false) return;
            if (Time.fixedTime - m_TimeStart > 3) return;
            Vector2 v = touch.position - m_ptStart;
            // 相对于起始点的距离。
            float len = v.magnitude;
            if (len < m_Len) return;
            float degree = Mathf.Rad2Deg * Mathf.Atan2(v.x, v.y);
            if (-45 >= degree && degree >= -135)
            {
                m_Flag = false;
                GC.Move_EventHandle(MoveDirection.Left);
            }
            else if (45 <= degree && degree <= 135)
            {
                m_Flag = false;
                GC.Move_EventHandle(MoveDirection.Right);
            }
            else if (-45 <= degree && degree <= 45)
            {
                m_Flag = false;
                GC.Move_EventHandle(MoveDirection.Up);
            }
            else if (135 <= degree || degree <= 135)
            {
                m_Flag = false;
                GC.Move_EventHandle(MoveDirection.Down);
            }
        }
    }
}
