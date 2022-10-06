using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDeath : MonoBehaviour
{
    public GameObject enemy;

    public void Destroy()
    {
        Destroy(enemy);
    }
}
