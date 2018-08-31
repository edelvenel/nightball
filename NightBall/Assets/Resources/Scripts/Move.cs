using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт движения для MovingPlatform
public class Move : MonoBehaviour {

    int dirX; // направление по оси x
    Rigidbody2D body;
    bool borderCheck; // отслеживание пересечения с границей экрана

    private void Start()
    {
        dirX = 1;
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        borderCheck = transform.position.x < -2.5f || transform.position.x > 2.5f;

        if (borderCheck)
        {
            if (transform.position.x > 2.5f)
            {
                dirX = -1;
            }
            else
            {
                dirX = 1;
            }
        }

        if (!borderCheck) return;
    }

    void Update ()
    {
        body.velocity = new Vector2(2 * dirX, body.velocity.y);
    }
}
