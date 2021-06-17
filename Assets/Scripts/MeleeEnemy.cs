using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Plane = UnityEngine.Plane;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MeleeEnemy : MonoBehaviour
{
    public int damage;
    public float attackCooldown;
        
    private GameObject player;
    private Player p;
    private Enemy enemy;
    private Vector2 goal;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p = player.GetComponent<Player>();
        enemy = GetComponent<Enemy>();
        goal = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = transform.position;
        Vector2 difference;
        if (playerPosition.x >= enemy.roomPosition.x - enemy.roomSize.x / 2 && 
            playerPosition.y >= enemy.roomPosition.y - enemy.roomSize.y / 2 &&
            playerPosition.x <= enemy.roomPosition.x + enemy.roomSize.x / 2 && 
            playerPosition.y <= enemy.roomPosition.y + enemy.roomSize.y / 2) 
        {
            difference.x = playerPosition.x - enemyPosition.x;
            difference.y = playerPosition.y - enemyPosition.y;
        }
        else
        {
            difference.x = goal.x - enemyPosition.x;
            difference.y = goal.y - enemyPosition.y;
            if ((difference.x > -1.1 && difference.x < 1.1 &&
                 difference.y > -1.1 && difference.y < 1.1) ||
                (goal.x == 0 && goal.y == 0))
            {
                goal.x = enemy.roomPosition.x + Random.Range(-enemy.roomSize.x / 2 + 1, enemy.roomSize.x / 2 - 1);
                goal.y = enemy.roomPosition.y + Random.Range(-enemy.roomSize.y / 2 + 1, enemy.roomSize.y / 2 - 1);
                difference.x = goal.x - enemyPosition.x;
                difference.y = goal.y - enemyPosition.y;
            }
        }
        if (difference.x > 1)
        {
            enemyPosition.x += 0.01f;
        }
        else if (difference.x < -1)
        {
            enemyPosition.x -= 0.01f;
        }
        if (difference.y > 1)
        {
            enemyPosition.y += 0.01f;
        }
        else if (difference.y < -1)
        {
            enemyPosition.y -= 0.01f;
        }
        if (difference.x > -1.1 && difference.x < 1.1 &&
            difference.y > -1.1 && difference.y < 1.1 &&
            timer >= attackCooldown)
        {
            p.TakeDamage(damage);
            timer = 0f;
        }
        transform.position = enemyPosition;
    }
}
