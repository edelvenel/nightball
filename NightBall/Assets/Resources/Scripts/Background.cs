using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    Transform player;
    bool playerInside;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update () {
        transform.position = new Vector3(transform.position.x, (player.transform.position.y - 11) / 2, transform.position.z);
    }
}

