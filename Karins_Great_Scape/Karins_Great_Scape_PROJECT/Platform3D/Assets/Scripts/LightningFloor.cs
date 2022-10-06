using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningFloor : MonoBehaviour
{
    float planeScale = 0.1f;

    void Awake()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);

        if (planeScale <= 0.435882f) planeScale += 0.1f;
        transform.localScale = new Vector3(planeScale, planeScale, planeScale);
    }

    public void ResetScale()
    {
        planeScale = 0.1f;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
