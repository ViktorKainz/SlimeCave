using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask whatIsSolid;
    
    
    // public GameObject destroyEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Must Take Dag");
            }
            DestroyProjectile();
        }
        
        
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    private void DestroyProjectile()
    {
        // Instantiate(destroyEffect, transform.position, Quaternion.identity);
        DestroyImmediate(gameObject);
    }
}
