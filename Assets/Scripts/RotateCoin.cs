using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{

    public float rotationSpeed = 12f;

    void Update()
    {
        transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
    }
}
