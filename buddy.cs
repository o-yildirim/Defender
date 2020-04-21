using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class buddy : MonoBehaviour
{
    private Rigidbody2D buddyRigidbody;
    private float currentHealth;
    private Vector2 speedVector;
    private bool hittingEnemy = false;
    private float timeSinceLastHit = 0f;
    public GameObject healthBar;
    private float reduceAmount;
    private float buddyMaxHealth;
    private float buddyMaxHBSize;
    public GameObject spear;
    public AudioSource trigger;
    public AudioClip spearSound;
    public GameObject enemyToHit;
    public bool isDead = false;
    void Start()
    {
        currentHealth = 80;
        buddyMaxHealth = currentHealth;
        buddyMaxHBSize = healthBar.transform.localScale.x;
        speedVector = new Vector2(1.2f, 0f);

        buddyRigidbody = GetComponent<Rigidbody2D>();
        buddyRigidbody.velocity = speedVector;

    }


    void Update()
    {
        timeSinceLastHit += Time.deltaTime;


       /* if (hittingEnemy && timeSinceLastHit >= 2.5f && !GameController.instance.isDead)
        {


            //GameController.instance.damagePlayerMelee();
            StartCoroutine(attack());
            timeSinceLastHit = 0f;



        }*/

        if(transform.position.x >= 9.5f)
        {
            buddyRigidbody.velocity = Vector2.zero;
        }
        if(enemyToHit != null && enemyToHit.GetComponent<Enemy>().isDead != true && hittingEnemy && timeSinceLastHit >= 2.5f && !GameController.instance.isDead && !isDead)
          {
            StartCoroutine(attack());
            timeSinceLastHit = 0f;
            if (enemyToHit.GetComponent<Enemy>().isDead == true)
            {
                hittingEnemy = false;
                enemyToHit = null;
            }
              
          }

    }

    public IEnumerator attack()
    {



        yield return new WaitForSeconds(1f);
        //spear.transform.Translate(spear.transform.right * 5 * Time.deltaTime);
        spear.transform.position = spear.transform.position + new Vector3(0.8f,0f,0f);
        if (!isDead)
        {
            trigger.PlayOneShot(spearSound);
        }
        if (enemyToHit != null && enemyToHit.GetComponent<Enemy>().isDead == false)
        {
            enemyToHit.GetComponent<Enemy>().takeDamageByBuddy();
        }
        yield return new WaitForSeconds(0.8f);
        //spear.transform.Translate(spear.transform.right * -5 * Time.deltaTime);
        spear.transform.position = spear.transform.position + new Vector3(-0.8f, 0f, 0f);


    }
    private void Die()
    {
        isDead = true;
        GameController.instance.deadBuddyCount++;
        Destroy(gameObject);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") && !isDead && hittingEnemy == false)
        {
            buddyRigidbody.velocity = Vector2.zero;
            hittingEnemy = true;
            enemyToHit = collision.gameObject;
            //StartCoroutine(attack());
        }
        /*if(collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(gameObject.GetComponent<Enemy>().knockback());
          
        }*/
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hittingEnemy = true;
            if (enemyToHit == null)
            {
                enemyToHit = collision.gameObject;
            }
            buddyRigidbody.velocity = Vector2.zero;
            /*while(enemyToHit.GetComponent<Enemy>().isDead != true && hittingEnemy && timeSinceLastHit >= 2.5f && !GameController.instance.isDead)
            {
                StartCoroutine(attack());
                timeSinceLastHit = 0;
            }
         */
        }
    }




            private void OnTriggerExit2D(Collider2D collision)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    hittingEnemy = false;
                    enemyToHit = null;
                    if (transform.position.x <= 11f) {
                        buddyRigidbody.velocity = speedVector;

                    }
                }
            }



            public void reduceHealthbar()
            {
                
                float healthBarXScale = healthBar.transform.localScale.x;
                Vector3 HbPos = healthBar.transform.position;

                healthBar.transform.localScale = new Vector3(healthBarXScale - reduceAmount, healthBar.transform.localScale.y, 0f);
                healthBar.transform.position = new Vector3(HbPos.x - (reduceAmount / 2), HbPos.y, 0f);

            }

            public void takeDamage()
            {
                currentHealth -= GameController.instance.enemyDamage;
                reduceAmount = buddyMaxHBSize * GameController.instance.enemyDamage / buddyMaxHealth;
                if (currentHealth <= 0)
                {
                    Die();
                }
                reduceHealthbar();


            }
             public void takeDamageFromRanged()
             {
                currentHealth -= GameController.instance.rangedEnemyDamage;
                reduceAmount = buddyMaxHBSize * GameController.instance.rangedEnemyDamage/ buddyMaxHealth;
                if (currentHealth <= 0)
                {
                    Die();
                }
                reduceHealthbar();


    }

}
   

