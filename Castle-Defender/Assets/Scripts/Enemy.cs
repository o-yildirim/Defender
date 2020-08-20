using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D enemyRigidbody;
    private float currentHealth;
    private Vector2 speedVector;
    private bool hittingBase = false;
    private float timeSinceLastHit = 0f;
    public GameObject healthBar;
    private float reduceAmount;
    private float enemyMaxHealth;
    private float enemyMaxHBSize;
    public GameObject sword;
    public AudioSource trigger;
    public AudioClip swordSound;
    public bool hittingBuddy = false;
    public GameObject buddyToAttack;
    public bool isDead = false;
    private Color original;
    void Start()
    {
        currentHealth = GameController.instance.enemyHealth + GameController.instance.timePassed * 2;
        enemyMaxHealth = currentHealth;
        enemyMaxHBSize = healthBar.transform.localScale.x ;
        speedVector = new Vector2(-1.6f, 0f);

       enemyRigidbody = GetComponent<Rigidbody2D>();
       enemyRigidbody.velocity = speedVector;
       original = gameObject.GetComponent<Renderer>().material.color;
}

   
    void Update()
    {
        timeSinceLastHit += Time.deltaTime;

       
        if (hittingBase && timeSinceLastHit >= 2.5f && !GameController.instance.isDead)
        {


            //GameController.instance.damagePlayerMelee();
            StartCoroutine(attackBase());
            timeSinceLastHit = 0f;
            
            
           
        }
        /* if (hittingBuddy && timeSinceLastHit >= 2.5f && !GameController.instance.isDead && buddyToAttack != null)
         {


             //GameController.instance.damagePlayerMelee();
             StartCoroutine(attackBuddy(buddyToAttack));
             timeSinceLastHit = 0f;



         }*/
        if (buddyToAttack != null &&buddyToAttack.GetComponent<buddy>().isDead != true && hittingBuddy && timeSinceLastHit >= 2.5f && !GameController.instance.isDead && !isDead)
        {
            StartCoroutine(attackBuddy(buddyToAttack));
            timeSinceLastHit = 0;

        }

    }

    public IEnumerator attackBase()
    {

  

        yield return new WaitForSeconds(1f);
        sword.transform.Rotate(sword.transform.forward ,  110f );
        transform.Rotate(transform.forward , 30f );
        trigger.PlayOneShot(swordSound);
        GameController.instance.damagePlayerMelee();
        yield return new WaitForSeconds(0.8f);
        sword.transform.Rotate(sword.transform.forward , -110f );
        transform.Rotate(transform.forward , -30f );
        

    }
    public IEnumerator attackBuddy(GameObject buddy)
    {



        yield return new WaitForSeconds(1f);
        sword.transform.Rotate(sword.transform.forward, 110f);
        transform.Rotate(transform.forward, 30f);
        if (!isDead)
        {
            trigger.PlayOneShot(swordSound);
        }
        if (buddyToAttack != null && buddy.GetComponent<buddy>().isDead == false )
        {
            buddy.GetComponent<buddy>().takeDamage();
        }
        yield return new WaitForSeconds(0.8f);
        sword.transform.Rotate(sword.transform.forward, -110f);
        transform.Rotate(transform.forward, -30f);


    }
    private void Die()
    {
        isDead = true;
        GameController.instance.giveGold();
        GameController.instance.giveScore();
        Destroy(gameObject);
    }

 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            takeDamage();
            if (!hittingBuddy)
            {
                StartCoroutine(knockback());
            }

        }
        if (collision.gameObject.CompareTag("Base"))
        {
            enemyRigidbody.velocity = Vector2.zero;
            hittingBase = true;
        }

        if (collision.gameObject.CompareTag("Buddy"))
        {
            enemyRigidbody.velocity = Vector2.zero;
            buddyToAttack = collision.gameObject;

            if (buddyToAttack != null & buddyToAttack.GetComponent<buddy>().isDead != true && !isDead)
            {
                hittingBuddy = true;
            }
        }
        /*if(collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(gameObject.GetComponent<Enemy>().knockback());
          
        }*/
    }

   /* private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Buddy")) {
            hittingBuddy = true;
            if (buddyToAttack == null)
            {
                buddyToAttack = collision.gameObject;
            }
            enemyRigidbody.velocity = Vector2.zero;
            while (buddyToAttack.GetComponent<buddy>().isDead != true && hittingBuddy && timeSinceLastHit >= 2.5f && !GameController.instance.isDead )
            {
              StartCoroutine(attackBuddy(buddyToAttack));
              timeSinceLastHit = 0;
            
            }
       
        }
    }*/


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Base"))
        {
            hittingBase = false;
        }
        if (collision.gameObject.CompareTag("Buddy"))
        {
            hittingBuddy = false;
            buddyToAttack = null;
            enemyRigidbody.velocity = speedVector;
        }
    }

    public IEnumerator knockback()
    {
        Vector2 knockBackVector = new Vector2(GameController.instance.knockBackForce, 0f);     
        enemyRigidbody.velocity = knockBackVector;
        yield return new WaitForSeconds(0.2f);
        if (!hittingBase)
        {
            enemyRigidbody.velocity = speedVector;
        }
    }

    public void reduceHealthbar()
    {
        //reduceAmount = enemyMaxHBSize * GameController.instance.playerDamage / enemyMaxHealth;
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
        reduceAmount = enemyMaxHBSize * GameController.instance.playerDamage / enemyMaxHealth;
        reduceHealthbar();
       
      
    }
    public void takeDamageByBuddy()
    {
        currentHealth -= enemyMaxHealth /3;
        StartCoroutine(flash(0.025f));
        if (currentHealth <= 0)
        {
            Die();
        }
        reduceAmount = (enemyMaxHBSize * (enemyMaxHealth/3)) / enemyMaxHealth;
        reduceHealthbar();


    }
    public IEnumerator flash(float flashDuration)
    {
     

        gameObject.GetComponent<Renderer>().material.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        gameObject.GetComponent<Renderer>().material.color = original;

       /* if (gameObject.GetComponent<Renderer>().material.color == original)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = original;
        }*/
    }
    

}
