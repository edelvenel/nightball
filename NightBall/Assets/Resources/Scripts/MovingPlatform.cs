using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : Platform {

    public MovingPlatform()
    {
        Data();
        platform.transform.position = new Vector3(0, 0, -2);
    }

    public MovingPlatform(float x, float y)
    {
        Data();
        platform.transform.position = new Vector3(x, y, -2);
    }

    void Data()
    {
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/MovingPlatform"));
        jumpHeight = 10;
        platform.name = "MovingPlatform";
    }
}
