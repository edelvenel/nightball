using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс мелков
public class Chalk {

    protected GameObject chalk;
    protected int points;

	public GameObject Exemplar
    {
        get { return chalk; }
    }

    public float PosX
    {
        get { return chalk.transform.position.x; }
        set { chalk.transform.position = new Vector3 (value, chalk.transform.position.y, chalk.transform.position.z); }
    }

    public float PosY
    {
        get { return chalk.transform.position.y; }
        set { chalk.transform.position = new Vector3 (chalk.transform.position.x, value, chalk.transform.position.z); }
    }

    public int Points
    {
        get { return points; }
    }
}
