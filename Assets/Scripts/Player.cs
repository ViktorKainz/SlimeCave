using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public int health;
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
            Debug.Log("Dead");
        }
    }
}
