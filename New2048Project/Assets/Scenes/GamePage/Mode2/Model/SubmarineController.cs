using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    GameController_Mode2 GC;
    void Start()
        =>GC = GameObject.Find("GameController").GetComponent<GameController_Mode2>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GC.isPause) return;
        
        GC.isPause = true;
        GC.IsLose = true;
    }

    private Vector3 LeftLimiter = new Vector3(-2f, 2.65f, 0f);
    private Vector3 RightLimiter = new Vector3(2f, 2.65f, 0f);
    public void Move_EventHandle(Vector3 targetPosition)
    {

        if (targetPosition.x < LeftLimiter.x)
            transform.position = LeftLimiter;
        else if (targetPosition.x > RightLimiter.x)
            transform.position = RightLimiter;
        else
            transform.position = new Vector3(targetPosition.x, 2.65f, 0f);
    }
    public void dropEntity()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<EntityControllerMode2>().dropEntity();
        }
    }
}
