using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс мелков "Зеленый мелок"
public class GreenChalk : Chalk
{
    public GreenChalk ()
    {
        Data ();
        chalk.transform.position = new Vector3 (0, 0, -2);
    }

    public GreenChalk (float x, float y)
    {
        Data();
        chalk.transform.position = new Vector3 (x, y, -2);
    }

    void Data()
    {
        chalk = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/GreenChalk"));
        points = 5;
        chalk.name = "GreenChalk";
        chalk.transform.parent = GameObject.Find("Chalks").transform;
    }
}
