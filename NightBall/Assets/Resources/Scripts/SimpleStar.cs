using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStar : Star {

	public SimpleStar()
    {
        Data();
        star.transform.position = new Vector3(0, 0, -2);
    }

    public SimpleStar(float x, float y)
    {
        Data();
        star.transform.position = new Vector3(x, y, -2);
    }

    void Data()
    {
        star = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/SimpleStar"));
        points = 1;
        star.name = "SimpleStar";
        star.transform.parent = GameObject.Find("Stars").transform;
    }
}
