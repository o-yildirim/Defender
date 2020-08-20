using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotgunShooting : MonoBehaviour
{



    public Transform firePointUp;
    public Transform firePointMiddle;
    public Transform firePointDown;
    public GameObject bulletPrefab;
    public GameObject shotGun;
    private float timeCounter;

    private GameObject bulletUp;
    private GameObject bulletDown;
    private GameObject bulletMiddle;

    private Vector2 direction;
    private float extraSpeed = 30f;
    public AudioClip shotgunBlast;
    public AudioSource soundMaker;
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (Input.GetMouseButton(0) && timeCounter >= GameController.instance.attackSpeed && GameController.instance.onResume)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shotGun.transform.position;

            //Bullet above
           
            bulletUp = Instantiate(bulletPrefab, firePointUp.transform.position, firePointUp.transform.rotation);
            bulletUp.transform.Rotate(0f, 0f, 90f);
            bulletUp.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y + 0.5f).normalized * extraSpeed;
            //Bullet middle
            bulletMiddle = Instantiate(bulletPrefab, firePointMiddle.transform.position, firePointMiddle.transform.rotation);
            bulletMiddle.GetComponent<Rigidbody2D>().velocity = direction.normalized * extraSpeed;

            //Bullet down
            soundMaker.PlayOneShot(shotgunBlast, GameController.instance.attackSpeed);
            bulletDown = Instantiate(bulletPrefab, firePointDown.transform.position, firePointDown.transform.rotation);
            bulletDown.transform.Rotate(0f, 0f, -90f);
            bulletDown.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x,direction.y-0.5f).normalized * extraSpeed;

            timeCounter = 0;
        }
    }


}


