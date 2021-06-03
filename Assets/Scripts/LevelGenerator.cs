using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public int roomCount;
    public int levelSize;

    public int width;
    public int height;
    
    public Tile door1;
    public Tile door2;

    private GameObject[,] plan;
    private GameObject[,] level;
    private Rooms rooms;
    
    // Start is called before the first frame update
    void Start()
    {
        plan = new GameObject[levelSize, levelSize];
        level = new GameObject[levelSize, levelSize];
        rooms = (Rooms) GameObject.FindGameObjectWithTag("Rooms").GetComponent(typeof(Rooms));
        plan[levelSize / 2, levelSize / 2] = rooms.startRoom;
        if (roomCount > levelSize * levelSize)
        {
            roomCount = levelSize * levelSize;
        }

        for (int r = 1; r < roomCount; r++)
        {
            foreach (int y in Enumerable.Range(0, levelSize).OrderBy(x => Random.Range(0, levelSize)))
            {
                foreach (int x in Enumerable.Range(0, levelSize).OrderBy(x => Random.Range(0, levelSize)))
                {
                    if ((plan[y, x] == null) && (
                        (y > 0 && plan[y - 1, x] != null) || //north
                        (x > 0 && plan[y, x - 1] != null) || //west
                        (y < plan.GetLength(0) - 1 && plan[y + 1, x] != null) || //south
                        (x < plan.GetLength(1) - 1 && plan[y, x + 1] != null))) //east
                    {
                        plan[y, x] = rooms.normalRooms[Random.Range(0, rooms.normalRooms.Length)];
                        goto Next;
                    }
                }
            }

            Next: ;
        }

        bool boss = true;
        while (boss)
        {
            var y = Random.Range(0, levelSize - 1);
            var x = Random.Range(0, levelSize - 1);
            if (plan[y, x] == null || plan[y, x] == rooms.startRoom) continue;
            if (y + 1 < levelSize && x + 1 < levelSize &&
                plan[y + 1, x] == null &&
                plan[y, x + 1] == null &&
                plan[y + 1, x + 1] == null)
            {
                plan[y, x] = rooms.cornerRooms[2];
                plan[y + 1, x] = rooms.cornerRooms[0];
                plan[y, x + 1] = rooms.cornerRooms[3];
                plan[y + 1, x + 1] = rooms.cornerRooms[1];
            }
            else if (y + 1 < levelSize && x > 0 &&
                     plan[y + 1, x] == null &&
                     plan[y, x - 1] == null &&
                     plan[y + 1, x - 1] == null)
            {
                plan[y, x] = rooms.cornerRooms[3];
                plan[y + 1, x] = rooms.cornerRooms[1];
                plan[y, x - 1] = rooms.cornerRooms[2];
                plan[y + 1, x - 1] = rooms.cornerRooms[0];
            }
            else if (y > 0 && x + 1 < levelSize && 
                     plan[y - 1, x] == null &&
                     plan[y, x + 1] == null &&
                     plan[y - 1, x + 1] == null)
            {
                plan[y, x] = rooms.cornerRooms[0];
                plan[y - 1, x] = rooms.cornerRooms[2];
                plan[y, x + 1] = rooms.cornerRooms[1];
                plan[y - 1, x + 1] = rooms.cornerRooms[3];
            }
            else if (y > 0 && x > 0 &&
                     plan[y - 1, x] == null &&
                     plan[y, x - 1] == null &&
                     plan[y - 1, x - 1] == null)
            {
                plan[y, x] = rooms.cornerRooms[1];
                plan[y - 1, x] = rooms.cornerRooms[3];
                plan[y, x - 1] = rooms.cornerRooms[0];
                plan[y - 1, x - 1] = rooms.cornerRooms[2];
            }
            else continue;
            boss = false;
        }

        for (int y = 0; y < levelSize; y++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                if (plan[y, x] != null)
                {
                    if (plan[y, x] == rooms.startRoom)
                    {
                        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(x * width, y * height, -1);
                    }
                    level[y, x] = Instantiate(plan[y, x], new Vector3(x * width, y * height), plan[y, x].transform.rotation);
                    Tilemap map = level[y, x].transform.GetChild(1).gameObject.GetComponent<Tilemap>();
                    BoundsInt bound = map.cellBounds;
                    if (y > 0 && plan[y - 1, x] != null && 
                        !(rooms.cornerRooms.Contains(plan[y, x]) && rooms.cornerRooms.Contains(plan[y - 1, x])))
                    {
                        map.SetTile(new Vector3Int(0, bound.yMin, 0), door1);
                        map.SetTile(new Vector3Int(-1, bound.yMin, 0), door2);
                    }

                    if (y < plan.GetLength(0) - 1 && plan[y + 1, x] != null && 
                        !(rooms.cornerRooms.Contains(plan[y, x]) && rooms.cornerRooms.Contains(plan[y + 1, x])))
                    {
                        map.SetTile(new Vector3Int(0, bound.yMax-1, 0), door2);
                        map.SetTile(new Vector3Int(-1, bound.yMax-1, 0), door1);
                    }

                    if (x > 0 && plan[y, x - 1] != null && 
                        !(rooms.cornerRooms.Contains(plan[y, x]) && rooms.cornerRooms.Contains(plan[y, x - 1])))
                    {
                        map.SetTile(new Vector3Int(bound.xMin, 0, 0), door2);
                        map.SetTile(new Vector3Int(bound.xMin, -1, 0), door1);
                    }

                    if (x < plan.GetLength(1) - 1 && plan[y, x + 1] != null && 
                        !(rooms.cornerRooms.Contains(plan[y, x]) && rooms.cornerRooms.Contains(plan[y, x + 1])))
                    {
                        map.SetTile(new Vector3Int(bound.xMax-1, 0, 0), door1);
                        map.SetTile(new Vector3Int(bound.xMax-1, -1, 0), door2);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}