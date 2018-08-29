using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Remoting;

public class PlayerControl : MonoBehaviour {

    readonly sbyte speed = 5; // скорость по оси x
    sbyte dirX = 0; //направление по оси x;
    Rigidbody2D body;
    public GameObject left;
    public GameObject right;

	void Start ()
    {

        Physics2D.IgnoreLayerCollision(0, 5);
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
        if ((Input.GetKey(KeyCode.D) || right.GetComponent<Touch>().IsTouched) && transform.position.x < 2.8)
        {
            dirX = 1;
            Move();
        }
        else if ((Input.GetKey(KeyCode.A) || left.GetComponent<Touch>().IsTouched) && transform.position.x > -2.8)
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
            //Исчезновение платформы
            Destroy(collision.collider);
            collision.collider.GetComponent<SpriteRenderer>().enabled = false;      
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Star")
        {
            string name = collision.name;
            ObjectHandle handle = Activator.CreateInstance("Assembly-CSharp", name);
            Star star = (Star)handle.Unwrap();
            Game.AddPoints(star.Points);
            Destroy(star.Exemplar);
            Destroy(collision);
            collision.GetComponent<SpriteRenderer>().enabled = false;
            collision.GetComponent<Light>().enabled = false;
        }
    }
}
