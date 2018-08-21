using UnityEngine;
using System.Collections;

public class BGStar
{
    GameObject obj;

    public BGStar()
    {
        Obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Stars" + Random.Range(1,4).ToString()));

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
