using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс звезд
public class Star {

    protected GameObject star;
    protected int points;

	public GameObject Exemplar
    {
        get { return star; }
    }

    public float PosX
    {
        get { return star.transform.position.x; }
        set { star.transform.position = new Vector3 (value, star.transform.position.y, star.transform.position.z); }
    }

    public float PosY
    {
        get { return star.transform.position.y; }
        set { star.transform.position = new Vector3 (star.transform.position.x, value, star.transform.position.z); }
    }

    public int Points
    {
        get { return points; }
    }
}
