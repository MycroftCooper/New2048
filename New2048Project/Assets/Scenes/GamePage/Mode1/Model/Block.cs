using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public EntityControllerMode1 Entity { get; set; }
    public Vector2Int LogicPosition { get; set;}
    public Vector3 ScreenPosition { get; set; }

    public Block(Vector2Int LogicPosition,Vector3 ScreenPosition)
    {
        Entity = null;
        this.LogicPosition = LogicPosition;
        this.ScreenPosition = ScreenPosition;
    }
    public bool isEmpty()=> Entity == null;

}