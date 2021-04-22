using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int direction;

    private RoomTemplates templates;
    private GameObject[][] rooms;
    private GameObject room;
    private int rand;
    private bool spawn = true;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.Equals(new Vector3(0, 0, 0)))
        {
            Destroy(gameObject);
        }
        else
        {
            templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
            rooms = new GameObject[][] {templates.south, templates.west, templates.nord, templates.east};
            Invoke("Spawn", 1f);
            Destroy(gameObject, 100f);
        }
    }

    void Spawn()
    {
        if (spawn)
        {
            rand = templates.roomsSpawned < templates.maxRooms
                ? templates.roomsSpawned < templates.minRooms ? Random.Range(1, rooms[direction].Length) :
                Random.Range(0, rooms[direction].Length)
                : 0;
            room = Instantiate(rooms[direction][rand], transform.position, rooms[direction][rand].transform.rotation);
            templates.roomsSpawned++;
            spawn = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint") && other.GetComponent<RoomSpawner>().spawn)
        {
            Destroy(other);
            if (!spawn)
            {
                Destroy(room);
            }
            spawn = false;
            int r = 0;
            for (int i = 1; i < rooms[direction].Length; i++)
            {
                for (int j = 1; j < rooms[other.GetComponent<RoomSpawner>().direction].Length; j++)
                {
                    if (rooms[direction][i].Equals(rooms[other.GetComponent<RoomSpawner>().direction][j]))
                    {
                        r = i;
                    }
                }
            }
            room = Instantiate(rooms[direction][r], transform.position, rooms[direction][r].transform.rotation);
            Debug.Log(room);
        }
    }
}