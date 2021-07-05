using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameArchive_Mode2;

public class EntityControllerMode2 : MonoBehaviour
{
    public int num;
    public int Num
    {
        get => num;
        set
        {
            num = value;
            setSpriteTexture();
        }
    }

    private GameController_Mode2 GC;
    private Rigidbody2D rb2d;
    private CircleCollider2D cc2d;

    public void setSpriteTexture()
    {
        string str = "imgs/Num/数字球_" + Num.ToString();
        List<Sprite> sprites = new List<Sprite>(Resources.LoadAll<Sprite>("imgs/Num/数字球"));
        Sprite Tex = sprites.Find(p=>p.name== "数字球_" + ((int)Mathf.Log(Num,2)-1).ToString()) ;
        gameObject.GetComponent<SpriteRenderer>().sprite = Tex;
        cc2d.radius = Tex.rect.width / 200f;
    }
    private static EntityControllerMode2 createEntity(Vector3 position)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);//实例化预制体的rotation，可自定义
        Transform parent = GameObject.Find("Submarine").transform;
        GameObject prefab = Resources.Load<GameObject>("Entity2");
        GameObject TheEntity = GameObject.Instantiate(prefab, position, rotation, parent);
        TheEntity.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        EntityControllerMode2 entityController = TheEntity.GetComponent<EntityControllerMode2>();
        entityController.GC = GameObject.Find("GameController").GetComponent<GameController_Mode2>();
        entityController.rb2d = TheEntity.GetComponent<Rigidbody2D>();
        entityController.rb2d.isKinematic = true;
        entityController.cc2d = TheEntity.GetComponent<CircleCollider2D>();
        return entityController;
    }
    public static EntityControllerMode2 createNewEntity(Vector3 submPosition)
    {
        EntityControllerMode2 entityController = createEntity(submPosition);
        float f = Random.Range(0, 1f);
        if ( f< 0.333f)
            entityController.Num = 2;
        else
            if(f<0.666f)
            entityController.Num = 4;
            else 
            entityController.Num = 8;
        return entityController;
    }
    public static EntityControllerMode2 createNewEntity(Vector3 submPosition, int num)
    {
        EntityControllerMode2 entityController = createEntity(submPosition);
        entityController.Num = num;
        entityController.dropEntity();
        return entityController;
    }
    public static void createNewEntitys(Dictionary<PositionVector3, int>entityList)
    {
        List<PositionVector3> keys = new List<PositionVector3>(entityList.Keys);
        foreach(PositionVector3 key in keys)
        {
            EntityControllerMode2.createNewEntity(new Vector3(key.x,key.y,key.z),entityList[key]);
        }
    }
    public void dropEntity()
    {
        transform.parent = null;
        rb2d.isKinematic = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Entity") return;
        if (collision.transform.parent != null || transform.parent!=null) return;
        if (collision.gameObject.GetComponent<EntityControllerMode2>().Num != Num) return;
        
        Vector3 newPosition = (transform.position + collision.transform.position) / 2;
        GC.entityCollisionHandle(newPosition,Num*2);
        GameObject.Destroy(collision.gameObject);
        GameObject.Destroy(gameObject);
    }
}
