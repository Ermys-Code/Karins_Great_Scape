using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    float currentRotation;

    void Update()
    {
        currentRotation += 100 * Time.deltaTime;
        transform.rotation = Quaternion.Euler(-90, currentRotation, 0);
        if (currentRotation >= 360) currentRotation = 0;
    }
}
