using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public void Start()
    {
        Vector3 originalPos = transform.position;
    }



    public IEnumerator shake()
    {
        Vector3 originalPos = transform.localPosition;
      
        float shakeDuration = 0.0f;
        float shakeTime = 0.15f;
        while(shakeDuration < shakeTime)
        {
            float xMagnitude = 8 + Random.Range(-0.25f, 0.25f);
            float yMagnitude = Random.Range(-0.25f, 0.25f);
            //transform.localPosition = new Vector3(xMagnitude, yMagnitude, originalPos.z);
            transform.position = new Vector3(xMagnitude, yMagnitude, originalPos.z);
            shakeDuration += Time.deltaTime;
            yield return null;
            
        }


        //transform.position = originalPos;
        transform.position = new Vector3(8f,0f,originalPos.z);

       

    }
}
