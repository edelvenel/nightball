using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для главной камеры
public class MainCamera : MonoBehaviour {

    Transform player;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>() ;
	}
	
	void Update ()
    {
        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
	}
}
