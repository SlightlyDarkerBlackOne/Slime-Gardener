using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ExplodeOnDeath : MonoBehaviour
{
    public GameObject explodeEffect;
    private float dropChance = 2f / 10f;

    public GameObject drop;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyHealthManager>().OnEnemyDeath += Explode;
    }

    void Explode(object sender, EventArgs e)
    {
        Instantiate(explodeEffect, transform.position, Quaternion.identity);
        if (Random.Range(0f, 1f) <= dropChance)
        {
            Instantiate(drop, transform.position, Quaternion.identity);
        }
    }
}