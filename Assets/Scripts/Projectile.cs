using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //public GameObject hitEffect;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        //GameOject effect = Instantiate(hitEffect, transform.position, Quaternion.identity)
        //Destroy(effect, 5f)
        Destroy(gameObject);
    }
}
