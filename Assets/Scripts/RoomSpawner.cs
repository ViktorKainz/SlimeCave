using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int direction;

    private RoomTemplates templates;
    private GameObject[][] rooms;
    private int rand;
    private bool spawn = true;
    
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        rooms = new GameObject[][] {templates.nord, templates.east, templates.south, templates.west};
        Invoke("Spawn",0.1f);
    }
    
    void Spawn()
    {
        if (spawn)
        {
            rand = templates.roomsSpawned < templates.maxRooms ? Random.Range(0, rooms[direction].Length) : 0;
            Instantiate(rooms[direction][rand], transform.position, rooms[direction][rand].transform.rotation);
            templates.roomsSpawned++;
            spawn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint") && !other.GetComponent<RoomSpawner>().spawn)
        {
            Destroy(gameObject);
        }
    }
}
