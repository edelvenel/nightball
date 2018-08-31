using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс платформ
public class Platform {

    protected GameObject platform;
    protected sbyte jumpHeight;

    public float PosX
    {
        get { return platform.transform.position.x; }
        set { platform.transform.position = new Vector3 (value, platform.transform.position.y, platform.transform.position.z); }
    }

    public float PosY
    {
        get { return platform.transform.position.y; }
        set { platform.transform.position = new Vector3 (platform.transform.position.x, value, platform.transform.position.z); }
    }

    public GameObject Exemplar
    {
        get { return platform; }
    }

    public sbyte Height
    {
        get { return jumpHeight; }
    }
}
