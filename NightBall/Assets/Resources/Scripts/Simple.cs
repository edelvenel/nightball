using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple : Platform {

    public Simple()
    {
        PosX = 0;
        PosY = 0;
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Simple"));
        TransPosition(0, 0, -2);
        jumpHeight = 10;
        SetName("Simple");
    }

    public Simple (float x, float y)
    {
        PosX = x;
        PosY = y;
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Simple"));
        TransPosition(x, y, -2);
        jumpHeight = 10;
        SetName("Simple");
    }
}
