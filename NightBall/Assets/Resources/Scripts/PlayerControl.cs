using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Remoting;

public class PlayerControl : MonoBehaviour {

    readonly sbyte speed = 5; // скорость по оси x
    sbyte dirX = 0; //направление по оси x;
    Rigidbody2D body;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        Jump(5);
	}
	
	void Update ()
    {
        //При движении вверх коллизии игнорируются
		if (body.velocity.y > 0)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        //Управление персонажем
        if (Input.GetKey(KeyCode.D) && transform.position.x < 2.5)
        {
            dirX = 1;
            Move();
        }
        else if (Input.GetKey(KeyCode.A) && transform.position.x > -2.5)
        {
            dirX = -1;
            Move();
        }
        else
        {
            dirX = 0;
            Stop();
        }
	}

    void Move()
    {
        if (dirX!=0)
        {
            body.velocity = new Vector2 (dirX * speed, body.velocity.y);
        }
    }

    void Stop()
    {
        body.velocity = new Vector2 (0, body.velocity.y);
    }

    void Jump(sbyte height)
    {
        body.velocity = new Vector2 (body.velocity.x, height);
    }

    void OnCollisionEnter2D (Collision2D collision)
    { 
        string name = collision.collider.name;
        ObjectHandle handle = Activator.CreateInstance("Assembly-CSharp", name);
        Platform platform = (Platform)handle.Unwrap();
        Jump(platform.Height);
        Destroy(platform.Exemplar);
    }
}
