using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс платформ "Фейк"
public class Fake : Platform
{

    public Fake()
    {
        Data();
        platform.transform.position = new Vector3(0, 0, -2);
    }

    public Fake(float x, float y)
    {
        Data();
        platform.transform.position = new Vector3(x, y, -2);
    }

    void Data()
    {
        platform = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Fake"));
        jumpHeight = 10;
        platform.name = "Fake";
    }
}