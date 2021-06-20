using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth;

    private int health;
    private Text healthCounter;
    
    private int floor = 1;
    private Text floorCounter;

    private LevelGenerator generator;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        floorCounter = GameObject.FindGameObjectWithTag("Floor").GetComponent<Text>();
        healthCounter = GameObject.FindGameObjectWithTag("Health").GetComponent<Text>();
        generator = GameObject.FindGameObjectWithTag("Generator").GetComponent<LevelGenerator>();
        generator.RemoveLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(int amount)
    {
        health--;
        if (health <= 0)
        {
            
            generator.GenerateLevel();
            health = maxHealth;
            floor = 1;
            floorCounter.text = "Floor: " + floor;
        }
        healthCounter.text = "Health: " + health;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Hole"))
        {
            generator.RemoveLevel();
            generator.GenerateLevel();
            floor++;
            floorCounter.text = "Floor: " + floor;
        }
    }
}
