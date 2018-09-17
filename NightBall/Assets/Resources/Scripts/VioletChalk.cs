using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Подкласс мелков "Фиолетовый мелок"
public class VioletChalk : Chalk {

    public VioletChalk ()
    {
        Data();
        chalk.transform.position = new Vector3 (0, 0, -2);
    }

    public VioletChalk (float x, float y)
    {
        Data();
        chalk.transform.position = new Vector3 (x, y, -2);
    }

    void Data ()
    {
        chalk = GameObject.Instantiate (Resources.Load<GameObject>("Prefabs/VioletChalk"));
        points = 10;
        chalk.name = "VioletChalk";
        chalk.transform.parent = GameObject.Find("Chalks").transform;
    }
}
