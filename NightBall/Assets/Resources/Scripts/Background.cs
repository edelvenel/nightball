using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для фона со звездами
public class Background : MonoBehaviour {

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update () {
        // Положение объекта по оси Y ниже относительно позиции Игрока на 11 (статичная высота объекта) и
        // ее изменение находится в зависимости 1/2 от перемещения Игрока по оси Y
        transform.position = new Vector3(transform.position.x, (player.transform.position.y - 11) / 2, transform.position.z);
    }
}

