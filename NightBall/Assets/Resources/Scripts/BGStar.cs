using UnityEngine;
using System.Collections;

public class BGStar
{
    GameObject obj;

    public BGStar(float y)
    {
        Obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Stars" + Random.Range(1,4).ToString()));
        Obj.transform.parent = GameObject.Find("BGStars").transform;
        obj.transform.position = new Vector3(0, y, -1);

    }

    public GameObject Obj
    {
        get
        {
            return obj;
        }

        set
        {
            obj = value;
        }
    }
}
