using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Platform {

    public Trampoline()
    {
        Data();
        platform.transform.position = new Vector3(0, 0, -2);
    }

    public Trampoline (float x, float y)
    {
        Data();
        platform.transform.position = new Vector3(x, y, -2);
    }

    void Data()
    {
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Trampoline"));
        jumpHeight = 15;
        platform.name = "Trampoline";
    }
}
