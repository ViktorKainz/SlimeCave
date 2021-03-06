using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public Vector2 roomPosition;
    public Vector2 roomSize;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        health--;
        animator.SetBool("hit", true);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
