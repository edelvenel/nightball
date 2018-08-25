using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCheck : MonoBehaviour
{
    LayerMask platform;
    bool collisionPlatform;

    void Awake()
    {
        platform = LayerMask.GetMask("Platform");
    }

    void FixedUpdate()
    {
        collisionPlatform = Physics2D.OverlapCircle(gameObject.transform.position, 0.31f, platform);
        if (collisionPlatform)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(0.5f, 1f), transform.position.z);
        }
        if (!collisionPlatform) return;
    }
}
