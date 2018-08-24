using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStar : Star {

    public SuperStar()
    {
        Data();
        star.transform.position = new Vector3(0, 0, -2);
    }

    public SuperStar(float x, float y)
    {
        Data();
        star.transform.position = new Vector3(x, y, -2);
    }

    void Data()
    {
        star = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/SuperStar"));
        points = 10;
        star.name = "SuperStar";
        star.transform.parent = GameObject.Find("Stars").transform;
    }
}
