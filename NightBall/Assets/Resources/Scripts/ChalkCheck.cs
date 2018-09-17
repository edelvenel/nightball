using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт проверки для объектов класса Chalk на пересечение с объектами класса Platform
public class ChalkCheck : MonoBehaviour
{
    LayerMask platform;
    bool collisionPlatform;

    void Awake ()
    {
        platform = LayerMask.GetMask("Platform");
    }

    void FixedUpdate ()
    {
        collisionPlatform = Physics2D.OverlapCircle (gameObject.transform.position, 0.31f, platform);

        if (collisionPlatform)
        {
            // Если обнаружено пересечение мелка с платформой, мелок перемещается на случайное расстояние от текущей позиции, в промежутке
            // от 0.5f до 1
            transform.position = new Vector3 (transform.position.x, transform.position.y + Random.Range(0.5f, 1f), transform.position.z);
        }

        if (!collisionPlatform) return;
    }
}
