using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputMode2 : MonoBehaviour, UserInput
{
    private GameController_Mode2 GC;
    void Start()
    {
        GC = GameObject.Find("GameController").GetComponent<GameController_Mode2>();
    }
    void Update()
    {
        PCInput();
        PhoneInput();
    }

    public void PCInput()
    {
        if(!GC.IsPause() && !GC.isShaking)
        {
            GC.moveSubmarine(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if(Input.GetMouseButtonDown(0))
                GC.createNewEntity();
            if (Input.GetMouseButtonUp(0))
                GC.dropTheEntity();
            if (Input.GetKeyDown(KeyCode.Space) )
            {
                GC.dropTheEntity();
                GC.shakePool();
            }
        }
    }

    private Touch touch;
    public void PhoneInput()
    {
        if(!GC.IsPause() && !GC.isShaking)
        {
            if (Input.touchCount != 1 && Input.touchCount != 2) return;
            touch = Input.GetTouch(0);
            if (Input.touchCount == 2)
            {
                GC.dropTheEntity();
                GC.shakePool();
                return;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                GC.moveSubmarine(Camera.main.ScreenToWorldPoint(touch.position));
            if (!GC.hasNewEntity() && touch.phase==TouchPhase.Began && Input.touchCount ==1)
                GC.createNewEntity();
            if (touch.phase == TouchPhase.Ended)
                GC.dropTheEntity();
        }
    }
}
