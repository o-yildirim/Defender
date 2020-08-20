using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAiming : MonoBehaviour
{

    private Vector3 mousePos;
    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.onResume) { 
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.eulerAngles = new Vector3(0f,0f,mousePos.y);
        //transform.LookAt(mousePos);
        
      
            var relativePos = mousePos - transform.position;
            var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            

            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            transform.rotation = rotation;

          
        }
     }
}
