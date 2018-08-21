using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Platform {

    public Trampoline()
    {
        PosX = 0;
        PosY = 0;
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Trampoline"));
        TransPosition(0, 0, -2);
        jumpHeight = 15;
        SetName("Trampoline");
    }

    public Trampoline (float x, float y)
    {
        PosX = x;
        PosY = y;
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Trampoline"));
        TransPosition(x, y, -2);
        jumpHeight = 15;
        SetName("Trampoline");
    }
}
