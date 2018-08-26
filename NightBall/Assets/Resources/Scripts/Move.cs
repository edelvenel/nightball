using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    int dirX;
    Rigidbody2D body;
    bool borderCheck;

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
            } else
            {
                dirX = 1;
            }
           
        }
        if (!borderCheck) return;
    }

    void Update () {

        body.velocity = new Vector2(2 * dirX, body.velocity.y);

    }
}
