using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttributes : MonoBehaviour
{
    void Start ()
    { 
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("EnemyRanged"))
        {
            Destroy(gameObject);
        }
    }

}
