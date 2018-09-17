﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Remoting;

// Главный скрипт управления (компонент Player)
public class PlayerControl : MonoBehaviour {

    readonly sbyte speed = 5; // скорость по оси x
    sbyte dirX = 0; // направление по оси x
    Rigidbody2D body;
    Animator anim;
    AudioSource aud;


    void Start ()
    {
        Physics2D.IgnoreLayerCollision (0, 5); // игнорирование коллизий слоя UI со слоем Default
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        aud = GetComponent<AudioSource>();
        Jump(5);
	}
	
	void Update ()
    {
        // При движении вверх коллизии игнорируются
		if (body.velocity.y > 0)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        // Управление персонажем
        if (((Input.touchCount == 1 && Input.GetTouch(0).position.x > Screen.width / 2) || Input.GetKey(KeyCode.D)) && transform.position.x < 2.8)
        {
            dirX = 1;
            Move();
        }
        else if (((Input.touchCount == 1 && Input.GetTouch(0).position.x < Screen.width / 2) || Input.GetKey(KeyCode.A)) && transform.position.x > -2.8)
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

    // Движение по оси x
    void Move ()
    {
        if (dirX!=0)
        {
            body.velocity = new Vector2 (dirX * speed, body.velocity.y);
        }
    }

    // Прекращение движения по оси x
    void Stop ()
    {
        body.velocity = new Vector2 (0, body.velocity.y);
    }

    // Прыжок
    void Jump (sbyte height)
    {
        body.velocity = new Vector2 (body.velocity.x, height);
        anim.Play("Player_Jump");
        aud.PlayOneShot(Resources.Load<AudioClip>("Sounds/Jump"));
    }

    // Отслеживание пересечений с платформами
    void OnCollisionEnter2D (Collision2D collision)
    {
        string name = collision.collider.name;
        ObjectHandle handle = Activator.CreateInstance("Assembly-CSharp", name);
        Platform platform = (Platform)handle.Unwrap();
        Jump(platform.Height);
        Destroy(platform.Exemplar);
        if (name == "Fake")
        {
            collision.collider.enabled = false;
            collision.collider.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Отслеживание пересечений с мелками
    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Chalk")
        {
            string name = collision.name;
            ObjectHandle handle = Activator.CreateInstance("Assembly-CSharp", name);
            Chalk chalk = (Chalk)handle.Unwrap();
            Game.AddPoints(chalk.Points);
            Destroy(chalk.Exemplar);
            Destroy(collision);
            collision.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
