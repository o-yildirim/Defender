using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D enemyRigidbody;
    private float currentHealth;
    private Vector2 speedVector;
  
    private float timeSinceLastHit = 0f;
    public GameObject healthBar;
    private float reduceAmount;
    private float enemyMaxHealth;
    private float enemyMaxHBSize;

    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public GameObject playerBase;
    public AudioClip fireSound;
    public AudioSource audioTrigger;
    Color original;
    void Start()
    {
        currentHealth = GameController.instance.enemyHealth -30 + GameController.instance.timePassed ;
        enemyMaxHealth = currentHealth;
        enemyMaxHBSize = healthBar.transform.localScale.x;
        speedVector = new Vector2(-0.8f, 0f);

        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyRigidbody.velocity = speedVector;
        original = gameObject.GetComponent<Renderer>().material.color;
    }


    void Update()
    {
        timeSinceLastHit += Time.deltaTime;


        if (transform.position.x <= 12f  && !GameController.instance.isDead)
        {


            if (timeSinceLastHit >= 2.2f)
            {
                StartCoroutine(shoot());
                timeSinceLastHit = 0f;
            }


        }

    }

    public IEnumerator flash(float flashDuration)
    {
        

        gameObject.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        gameObject.GetComponent<Renderer>().material.color = original;
    }
    private void Die()
    {
        GameController.instance.giveGold();
        GameController.instance.giveScore();
        Destroy(gameObject);
    }

    public IEnumerator shoot()
    {
        
        enemyRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.2f);
        Instantiate(enemyBulletPrefab, firePoint.transform.position,Quaternion.identity);
        audioTrigger.PlayOneShot(fireSound);
        Vector2 knockBackVector = new Vector2(10f, 0f);
        enemyRigidbody.velocity = knockBackVector;
        yield return new WaitForSeconds(0.25f);
        enemyRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.2f);
        enemyRigidbody.velocity = speedVector;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            takeDamage();



        }
        
        /*if(collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(gameObject.GetComponent<Enemy>().knockback());
          
        }*/
    }


    public void reduceHealthbar()
    {
        reduceAmount = enemyMaxHBSize * GameController.instance.playerDamage / enemyMaxHealth;
        float healthBarXScale = healthBar.transform.localScale.x;
        Vector3 HbPos = healthBar.transform.position;

        healthBar.transform.localScale = new Vector3(healthBarXScale - reduceAmount, healthBar.transform.localScale.y, 0f);
        healthBar.transform.position = new Vector3(HbPos.x - (reduceAmount / 2), HbPos.y, 0f);

    }

    public void takeDamage()
    {
        currentHealth -= GameController.instance.playerDamage;
        StartCoroutine(flash(0.025f));
        if (currentHealth <= 0)
        {
            Die();
        }
        reduceHealthbar();

    }

}
