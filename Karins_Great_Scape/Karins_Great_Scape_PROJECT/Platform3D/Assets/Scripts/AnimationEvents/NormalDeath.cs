using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalDeath : MonoBehaviour
{
    public GameObject enemy;

    public void Death()
    {
        Destroy(enemy.gameObject);
    }
}
