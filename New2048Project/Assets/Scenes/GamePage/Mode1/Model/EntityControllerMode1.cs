using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController_Mode1;

public class EntityControllerMode1 : MonoBehaviour
{
    public float moveSpeed;
    public float scaleSpeed;
    public float maxTime=3;
    public bool isChanged;
    private int num;
    public int Num
    {
        get => num;
        set
        {
            num = value;
            setSpriteTexture();
        }
    }
    public bool isOnPosition(Vector3 v3)
    {
        if (transform.localPosition.x == v3.x && transform.localPosition.y == v3.y)
            return true;
        return false;
    }
    private Vector3 targetScale = new Vector3(0.58f, 0.58f, 1);

    public void doEntityMove(Block block)
        => StartCoroutine( TransfomTools.moveToPosition(transform,block.ScreenPosition,moveSpeed));
    public void doEntityScale()
       => StartCoroutine(TransfomTools.Scale(transform, targetScale, scaleSpeed));
    //设置材质
    public void setSpriteTexture()
    {
        Sprite Tex = Resources.Load<Sprite>("imgs/Num/"+Num.ToString());
        gameObject.GetComponent<SpriteRenderer>().sprite = Tex;
    }
    private static EntityControllerMode1 createEntity(Vector3 position)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义
        Transform parent = GameObject.Find("Map").transform;
        GameObject prefab = Resources.Load<GameObject>("Entity");
        GameObject TheEntity = GameObject.Instantiate(prefab, position, rotation, parent);
        TheEntity.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        EntityControllerMode1 entityController = TheEntity.GetComponent<EntityControllerMode1>();
        entityController.doEntityScale();
        return entityController;
    }
    public static EntityControllerMode1 createNewEntity(Block block)
    {
        EntityControllerMode1 entityController = createEntity(block.ScreenPosition);
        if (Random.Range(0, 1f) < 0.5)
            entityController.Num = 2;
        else
            entityController.Num = 4;
        return entityController;
    }
    public static EntityControllerMode1 createNewEntity(Block block,int number)
    {
        EntityControllerMode1 entityController = createEntity(block.ScreenPosition);
        entityController.Num = number;
        return entityController;
    }
    public static void resetAllEntity()
    {
        GameObject[] entitys = GameObject.FindGameObjectsWithTag("Entity");
        for(int i=0;i<entitys.Length;i++)
        {
            entitys[i].GetComponent<EntityControllerMode1>().isChanged = false;
        }
    }
}
