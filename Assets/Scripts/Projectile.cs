using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEffect;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);

        if (other.gameObject.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(1);
        }
        
        Destroy(gameObject);
    }
}
