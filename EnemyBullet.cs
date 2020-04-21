using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float rotateSpeed = 50f;
    private Rigidbody2D bulletRigidbody;

    public void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = new Vector2(-8f, 0f);
    }
    public void Update()
    {
        transform.Rotate(transform.right * Time.deltaTime * rotateSpeed);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Base"))
        {          
            GameController.instance.damagePlayerRanged();
            Destroy(gameObject);          
        }
        if (collision.CompareTag("Buddy"))
        {
            collision.gameObject.GetComponent<buddy>().takeDamage();
            Destroy(gameObject);
        }
        
    }
}
