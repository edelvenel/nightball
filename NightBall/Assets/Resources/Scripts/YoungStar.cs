using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс звезд "Молодая звезда"
public class YoungStar : Star
{
    public YoungStar ()
    {
        Data();
        star.transform.position = new Vector3 (0, 0, -2);
    }

    public YoungStar (float x, float y)
    {
        Data();
        star.transform.position = new Vector3 (x, y, -2);
    }

    void Data()
    {
        star = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/YoungStar"));
        points = 3;
        star.name = "YoungStar";
        star.transform.parent = GameObject.Find("Stars").transform;
    }
}
