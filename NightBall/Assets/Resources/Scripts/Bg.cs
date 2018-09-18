using UnityEngine;
using System.Collections;

// Класс фонов
public class Bg
{
    GameObject obj;

    public Bg(float y)
    {
        // Создается объект Bg
        Obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Background"));
        Obj.transform.parent = GameObject.Find("BG").transform;
        obj.transform.position = new Vector3(0, y, 0);
    }

    public GameObject Obj
    {
        get { return obj; }
        set { obj = value; }
    }
}
