using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour 
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = transform.position;
        float xdifference = playerPosition.x - enemyPosition.x;
        float ydifference = playerPosition.y - enemyPosition.y;
        if (Math.Abs(xdifference) < 10 && Math.Abs(ydifference) < 10) 
        {
            if (xdifference > 1)
            {
                enemyPosition.x += 0.01f;
            }
            else if (xdifference < 1)
            {
                enemyPosition.x -= 0.01f;
            }
            if (ydifference > 1)
            {
                enemyPosition.y += 0.01f;
            }
            else if (ydifference < 1)
            {
                enemyPosition.y -= 0.01f;
            }
            transform.position = enemyPosition;
        }
    }
}
