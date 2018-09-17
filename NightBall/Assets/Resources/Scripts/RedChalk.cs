using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс мелков "Красный мелок"
public class RedChalk : Chalk
{
    public RedChalk ()
    {
        Data();
        chalk.transform.position = new Vector3 (0, 0, -2);
    }

    public RedChalk (float x, float y)
    {
        Data();
        chalk.transform.position = new Vector3 (x, y, -2);
    }

    void Data()
    {
        chalk = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/RedChalk"));
        points = 3;
        chalk.name = "RedChalk";
        chalk.transform.parent = GameObject.Find("Chalks").transform;
    }
}
