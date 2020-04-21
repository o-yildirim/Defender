using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject standartGun;

    
    private float timeCounter;
    private GameObject bulletToFire;
    private Vector2 direction;
    private float extraSpeed = 30f;
    public AudioClip blop;
    public AudioSource soundMaker;
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (Input.GetMouseButton(0) && timeCounter >= GameController.instance.attackSpeed && GameController.instance.onResume)
        {
            direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - standartGun.transform.position;

            bulletToFire = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
            soundMaker.PlayOneShot(blop,GameController.instance.attackSpeed);
            bulletToFire.GetComponent<Rigidbody2D>().velocity = direction.normalized * extraSpeed;
            timeCounter = 0;
        }
    }

    
}
