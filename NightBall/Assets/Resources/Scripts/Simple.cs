using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс платформ "Простая"
public class Simple : Platform {

    public Simple ()
    {
        Data();
        platform.transform.position = new Vector3 (0, 0, -2);
    }

    public Simple (float x, float y)
    {
        Data();
        platform.transform.position = new Vector3 (x, y, -2);
    }

    void Data ()
    {
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Simple"));
        jumpHeight = 10;
        platform.name = "Simple";
    }
}
