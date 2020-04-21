using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Texture2D aimTexture;
    public Transform[] spawnPoints;
    public Transform[] buddySpawnPoints;
    public GameObject enemyPrefab;
    public GameObject enemyRangedPrefab;
    [SerializeField]
    public float playerMaxHealth = 200;
    public float playerCurrentHealth = 200;
    public int playerDamage = 20;
    public int playerGold = 0;
    public int playerScore = 0;
    public float knockBackForce = 1f;
    public float attackSpeed = 0.25f;
    public float enemySpawnRate = 1.5f;
    public float enemyHealth = 100f;
    public Text goldText;
    public Text scoreText;
    public Text healthText;
    public bool isDead = false;
    public GameObject shopMenu;
    public GameObject pauseMenu;
    public GameObject deadScreen;
    public GameObject TutorialInfo;
    public GameObject healthBar;
    public bool onPause = false;
    public bool onShop = false;
    public bool onResume;
    public float reduceAmount;
    public float enemyDamage = 3.0f;
    public float rangedEnemyDamage = 7.0f;
    public float timeSinceSpawn = 0f;
    public Camera mainCamera;
    public float hbOriginal;
    public AudioSource gameAudio;
    public AudioClip intro;
    public AudioClip loop;
    public AudioClip takeDamage;
    public float timePassed = 0f;
    public int deadBuddyCount = 0;
    public Button buddyB;
    public GameObject buddyPrefab;
    public Text buddyText;
    public Text highScoreText;
    public Text buddyCostT;

    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
        


    }

    void Start()
    {
        Time.timeScale = 0f;
        onResume = false;
        gameAudio.PlayOneShot(intro);
        hbOriginal = healthBar.transform.localScale.x;
        highScoreText.text ="High Score " + PlayerPrefs.GetInt("HighScore").ToString();
        spawnEnemy();
      

    }

    public void summonBuddies()
    {
        deadBuddyCount = 0;
        for (int i = 0; i < buddySpawnPoints.Length; i++)
        {
            Instantiate(buddyPrefab, buddySpawnPoints[i].transform.position, buddySpawnPoints[i].transform.rotation);
        }
    }

    private void Update()
    {

        /* if(onResume && !isDead)
         {
             StartCoroutine(SpawnWave());
         }*/

        /*playerScore += (int)Time.time;
        scoreText.text = playerScore.ToString();
        */
        if (Input.GetKeyDown(KeyCode.E) && !isDead && onResume && !onPause)
        {
            StartCoroutine(shop());

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isDead && onResume && !onShop)
        {
            pause();
        }
        else if (Input.GetKeyDown(KeyCode.H) && !isDead)
        {
            playerGold += 9999;
            giveGold();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !isDead && !onResume && !onShop && onPause )
        {
            resumeFromPause();
        }

        else if(Input.GetKeyDown(KeyCode.E) && !onResume && onShop && !onPause)
       {
           resumeFromShop();
       }




        timePassed += Time.deltaTime;
        timeSinceSpawn += Time.deltaTime;

        if (timePassed > 10f && onResume && enemySpawnRate >= 0.3f)
        {
            timePassed = 0f;
            enemySpawnRate -= 0.15f;
        }

        if (timeSinceSpawn >= enemySpawnRate && !isDead && onResume)
        {

            spawnEnemy();
            timeSinceSpawn = 0f;
        }


        if (!gameAudio.isPlaying)
        {
            gameAudio.PlayOneShot(loop);
        }
        if(deadBuddyCount == 5)
        {
            buddyB.gameObject.SetActive(true);
            buddyText.text = "";
            buddyCostT.enabled = true;
        }

    }



    /* public IEnumerator SpawnWave()
     {

         int spawnPoint;


         while(!isDead && onResume)
         {
             spawnPoint = Random.Range(0, spawnPoints.Length );
             Instantiate(enemyPrefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation);
             yield return new WaitForSeconds(enemySpawnRate);
         }
     }*/
    public void spawnEnemy()
    {
        int spawnPoint;
        
         spawnPoint = Random.Range(0, spawnPoints.Length);
        if (playerScore >= 100) {
           int rollForSpawn = (int)Random.Range(0, 5);
            if (rollForSpawn < 1)
            {
               GameObject created = Instantiate(enemyRangedPrefab, spawnPoints[spawnPoint].transform.position, Quaternion.identity);
                //created.transform.Rotate(transform.up, 180f);
                //created.transform.Rotate(transform.right, 180f);
                created.transform.Rotate(transform.forward, 180f);
            }
            else
            {
                Instantiate(enemyPrefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation);
            }
        }
        else
        {
            Instantiate(enemyPrefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation);
        }

    }
    

    public void giveGold()
    {
        playerGold += Random.Range(2, 5);
        goldText.text = playerGold.ToString();
    }
    public void giveScore()
    {
        playerScore += Random.Range(10,20);
        scoreText.text = "Score: " + playerScore.ToString();
        if(playerScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
        }
    }

    public IEnumerator shop()
    {
        Time.timeScale = 0f;
        shopMenu.SetActive(true);
        
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        onShop = true;
        onResume = false;
        yield return null;
    }
    public void resumeFromShop()
    {      
        
        shopMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.SetCursor(aimTexture, Vector2.zero, CursorMode.Auto);
        onResume = true;
        onShop = false;
    }

    public void pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        onPause = true;
        onResume = false;
        
    }
    

    public void gameOver()
    {
        Time.timeScale = 0f;
        deadScreen.SetActive(true);
        isDead = true;
        onResume = false;
       
    }

    public void resumeFromPause()
    {

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.SetCursor(aimTexture, Vector2.zero, CursorMode.Auto);
        onResume = true;
        onPause = false;
    }

    public void quitApplication()
    {
        Application.Quit();
    }
    
    public void restart()
    {
        
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartTheGame();
    }

    public void StartTheGame()
    {
        TutorialInfo.SetActive(false);
        onResume = true;
        Time.timeScale = 1f;
        Cursor.SetCursor(aimTexture, Vector2.zero, CursorMode.Auto);
        //StartCoroutine(SpawnWave());

    }

    public void damagePlayerMelee()
    {

       playerCurrentHealth -= enemyDamage;
        healthText.text = playerCurrentHealth + "/" + playerMaxHealth;
        if (playerCurrentHealth <= 0f)
        {
            Die();
        }
        else
        {
            reduceAmount = hbOriginal * enemyDamage / playerMaxHealth;
            reduceHealthbar();
        }
    }
    public void damagePlayerRanged()
    {

        playerCurrentHealth -= rangedEnemyDamage;
        healthText.text = playerCurrentHealth + "/" + playerMaxHealth;
        gameAudio.PlayOneShot(takeDamage);
        StartCoroutine(mainCamera.GetComponent<CameraScript>().shake());
        
        if (playerCurrentHealth <= 0f)
        {
            Die();
        }
        else
        {
            reduceAmount = hbOriginal * rangedEnemyDamage / playerMaxHealth;
            reduceHealthbar();
        }
    }

    public void reduceHealthbar()
    {

        float healthBarXScale = healthBar.transform.localScale.x;
        Vector3 HbPos = healthBar.transform.position;

        healthBar.transform.localScale = new Vector3(healthBarXScale - reduceAmount, healthBar.transform.localScale.y, 0f);
        healthBar.transform.position = new Vector3(HbPos.x - (reduceAmount / 2), HbPos.y, 0f);




    }
    public void Die()
    {
        playerCurrentHealth = 0;
        healthBar.SetActive(false);
        healthText.text = 0 + "/" + playerMaxHealth;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gameOver();
    }

}
