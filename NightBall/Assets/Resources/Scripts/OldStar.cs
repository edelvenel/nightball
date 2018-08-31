using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс звезд "Старая звезда"
public class OldStar : Star
{
    public OldStar ()
    {
        Data ();
        star.transform.position = new Vector3 (0, 0, -2);
    }

    public OldStar (float x, float y)
    {
        Data();
        star.transform.position = new Vector3 (x, y, -2);
    }

    void Data()
    {
        star = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/OldStar"));
        points = 5;
        star.name = "OldStar";
        star.transform.parent = GameObject.Find("Stars").transform;
    }
}
