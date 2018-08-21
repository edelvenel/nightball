using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform {

    float posX;
    float posY;
    protected GameObject platform;
    protected sbyte jumpHeight;

    public float PosX
    {
        get { return posX; }
        set { posX = value; }
    }

    public float PosY
    {
        get { return posY; }
        set { posY = value; }
    }

    public GameObject Exemplar
    {
        get { return platform; }
    }

    public void TransPosition (float x, float y, float z)
    {
        platform.transform.position = new Vector3(x, y, z);
    }

    public sbyte Height
    {
        get { return jumpHeight; }
    }

    protected void SetName(string name)
    {
        platform.name = name;
    }
}
