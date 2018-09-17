using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс мелков "Простой мелок"
public class SimpleChalk : Chalk {

	public SimpleChalk ()
    {
        Data();
        chalk.transform.position = new Vector3 (0, 0, -2);
    }

    public SimpleChalk (float x, float y)
    {
        Data();
        chalk.transform.position = new Vector3 (x, y, -2);
    }

    void Data ()
    {
        chalk = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/SimpleChalk"));
        points = 1;
        chalk.name = "SimpleChalk";
        chalk.transform.parent = GameObject.Find("Chalks").transform;
    }
}
