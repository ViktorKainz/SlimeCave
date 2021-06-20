using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public struct Room
{
    public GameObject plan;
    public GameObject room;
    public List<GameObject> enemies;
    public bool cleared;
}

public class LevelGenerator : MonoBehaviour
{
    public int roomCount;
    public int levelSize;
    public int width;
    public int height;
    public int maxEnemies;
    public bool boss;
    public Tile door1;
    public Tile door2;

    private Room[,] level;
    private Rooms rooms;

    // Start is called before the first frame update
    void Start()
    {
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Rooms>();
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        level = new Room[levelSize, levelSize];
        level[levelSize / 2, levelSize / 2].plan = rooms.startRoom;
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
                    if ((level[y, x].plan == null) && (
                        (y > 0 && level[y - 1, x].plan != null) || //north
                        (x > 0 && level[y, x - 1].plan != null) || //west
                        (y < level.GetLength(0) - 1 && level[y + 1, x].plan != null) || //south
                        (x < level.GetLength(1) - 1 && level[y, x + 1].plan != null))) //east
                    {
                        level[y, x].plan = rooms.normalRooms[Random.Range(0, rooms.normalRooms.Length)];
                        goto Next;
                    }
                }
            }

            Next: ;
        }

        int maxDistance = 0;
        int maxX = 0;
        int maxY = 0;
        foreach (int y in Enumerable.Range(0, levelSize).OrderBy(x => Random.Range(0, levelSize)))
        {
            foreach (int x in Enumerable.Range(0, levelSize).OrderBy(x => Random.Range(0, levelSize)))
            {
                if (level[y, x].plan != null)
                {
                    int distance = Math.Abs(levelSize / 2 - y) + Math.Abs(levelSize / 2 - x);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        maxX = x;
                        maxY = y;
                    }
                }
            }
        }

        level[maxY, maxX].plan = rooms.holeRoom;

        while (boss)
        {
            var y = Random.Range(0, levelSize - 1);
            var x = Random.Range(0, levelSize - 1);
            if (level[y, x].plan == null || level[y, x].plan == rooms.startRoom) continue;
            if (y + 1 < levelSize && x + 1 < levelSize &&
                level[y + 1, x].plan == null &&
                level[y, x + 1].plan == null &&
                level[y + 1, x + 1].plan == null)
            {
                level[y, x].plan = rooms.cornerRooms[2];
                level[y + 1, x].plan = rooms.cornerRooms[0];
                level[y, x + 1].plan = rooms.cornerRooms[3];
                level[y + 1, x + 1].plan = rooms.cornerRooms[1];
            }
            else if (y + 1 < levelSize && x > 0 &&
                     level[y + 1, x].plan == null &&
                     level[y, x - 1].plan == null &&
                     level[y + 1, x - 1].plan == null)
            {
                level[y, x].plan = rooms.cornerRooms[3];
                level[y + 1, x].plan = rooms.cornerRooms[1];
                level[y, x - 1].plan = rooms.cornerRooms[2];
                level[y + 1, x - 1].plan = rooms.cornerRooms[0];
            }
            else if (y > 0 && x + 1 < levelSize &&
                     level[y - 1, x].plan == null &&
                     level[y, x + 1].plan == null &&
                     level[y - 1, x + 1].plan == null)
            {
                level[y, x].plan = rooms.cornerRooms[0];
                level[y - 1, x].plan = rooms.cornerRooms[2];
                level[y, x + 1].plan = rooms.cornerRooms[1];
                level[y - 1, x + 1].plan = rooms.cornerRooms[3];
            }
            else if (y > 0 && x > 0 &&
                     level[y - 1, x].plan == null &&
                     level[y, x - 1].plan == null &&
                     level[y - 1, x - 1].plan == null)
            {
                level[y, x].plan = rooms.cornerRooms[1];
                level[y - 1, x].plan = rooms.cornerRooms[3];
                level[y, x - 1].plan = rooms.cornerRooms[0];
                level[y - 1, x - 1].plan = rooms.cornerRooms[2];
            }
            else continue;

            boss = false;
        }

        for (int y = 0; y < levelSize; y++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                if (level[y, x].plan != null)
                {
                    if (level[y, x].plan == rooms.startRoom)
                    {
                        GameObject.FindGameObjectWithTag("Player").transform.position =
                            new Vector3(x * width, y * height, -1);
                    }

                    level[y, x].room = Instantiate(level[y, x].plan, new Vector3(x * width, y * height),
                        level[y, x].plan.transform.rotation);
                    level[y, x].room.transform.parent = transform;
                    var enemieCount = Random.Range(1, maxEnemies);
                    level[y, x].enemies = new List<GameObject>();
                    level[y, x].cleared = false;
                    for (int i = 0; i < enemieCount; i++)
                    {
                        GameObject enemy = rooms.enemies[Random.Range(0, rooms.enemies.Length)];
                        Enemy e = enemy.GetComponent<Enemy>();
                        e.roomPosition = new Vector2(x * width, y * height);
                        e.roomSize = new Vector2(width, height);
                        GameObject instance = Instantiate(enemy,
                            new Vector3(x * width + Random.Range(-width / 2 + 1, width / 2 - 1),
                                y * height + Random.Range(-height / 2 + 1, height / 2 - 1), -1),
                            enemy.transform.rotation);
                        instance.transform.parent = level[y, x].room.transform;
                        level[y, x].enemies.Add(instance);
                    }

                    Tilemap map = level[y, x].room.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
                    BoundsInt bound = map.cellBounds;
                    if (y > 0 && level[y - 1, x].plan != null &&
                        !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                          rooms.cornerRooms.Contains(level[y - 1, x].plan)))
                    {
                        map.SetTile(new Vector3Int(0, bound.yMin, 0), door1);
                        map.SetTile(new Vector3Int(-1, bound.yMin, 0), door2);
                    }

                    if (y < level.GetLength(0) - 1 && level[y + 1, x].plan != null &&
                        !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                          rooms.cornerRooms.Contains(level[y + 1, x].plan)))
                    {
                        map.SetTile(new Vector3Int(0, bound.yMax - 1, 0), door2);
                        map.SetTile(new Vector3Int(-1, bound.yMax - 1, 0), door1);
                    }

                    if (x > 0 && level[y, x - 1].plan != null &&
                        !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                          rooms.cornerRooms.Contains(level[y, x - 1].plan)))
                    {
                        map.SetTile(new Vector3Int(bound.xMin, 0, 0), door2);
                        map.SetTile(new Vector3Int(bound.xMin, -1, 0), door1);
                    }

                    if (x < level.GetLength(1) - 1 && level[y, x + 1].plan != null &&
                        !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                          rooms.cornerRooms.Contains(level[y, x + 1].plan)))
                    {
                        map.SetTile(new Vector3Int(bound.xMax - 1, 0, 0), door1);
                        map.SetTile(new Vector3Int(bound.xMax - 1, -1, 0), door2);
                    }
                }
                else
                {
                    level[y, x].cleared = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int y = 0; y < levelSize; y++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                if (!level[y, x].cleared)
                {
                    level[y, x].cleared = true;
                    foreach (var e in level[y, x].enemies)
                    {
                        if (e != null)
                        {
                            level[y, x].cleared = false;
                            break;
                        }
                    }

                    if (level[y, x].cleared)
                    {
                        Tilemap map = level[y, x].room.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
                        BoundsInt bound = map.cellBounds;
                        //Bottom
                        if (y > 0 && level[y - 1, x].plan != null &&
                            !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                              rooms.cornerRooms.Contains(level[y - 1, x].plan)))
                        {
                            map.SetTile(new Vector3Int(0, bound.yMin, 0), null);
                            map.SetTile(new Vector3Int(-1, bound.yMin, 0), null);
                            Tilemap b_map = level[y - 1, x].room.transform.GetChild(1).gameObject
                                .GetComponent<Tilemap>();
                            BoundsInt b_bound = map.cellBounds;
                            b_map.SetTile(new Vector3Int(0, b_bound.yMax - 1, 0), null);
                            b_map.SetTile(new Vector3Int(-1, b_bound.yMax - 1, 0), null);
                        }

                        //Top
                        if (y < level.GetLength(0) - 1 && level[y + 1, x].plan != null &&
                            !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                              rooms.cornerRooms.Contains(level[y + 1, x].plan)))
                        {
                            map.SetTile(new Vector3Int(0, bound.yMax - 1, 0), null);
                            map.SetTile(new Vector3Int(-1, bound.yMax - 1, 0), null);
                            Tilemap t_map = level[y + 1, x].room.transform.GetChild(1).gameObject
                                .GetComponent<Tilemap>();
                            BoundsInt t_bound = map.cellBounds;
                            t_map.SetTile(new Vector3Int(0, t_bound.yMin, 0), null);
                            t_map.SetTile(new Vector3Int(-1, t_bound.yMin, 0), null);
                        }

                        //Left
                        if (x > 0 && level[y, x - 1].plan != null &&
                            !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                              rooms.cornerRooms.Contains(level[y, x - 1].plan)))
                        {
                            map.SetTile(new Vector3Int(bound.xMin, 0, 0), null);
                            map.SetTile(new Vector3Int(bound.xMin, -1, 0), null);
                            Tilemap l_map = level[y, x - 1].room.transform.GetChild(1).gameObject
                                .GetComponent<Tilemap>();
                            BoundsInt l_bound = map.cellBounds;
                            l_map.SetTile(new Vector3Int(l_bound.xMax - 1, 0, 0), null);
                            l_map.SetTile(new Vector3Int(l_bound.xMax - 1, -1, 0), null);
                        }

                        if (x < level.GetLength(1) - 1 && level[y, x + 1].plan != null &&
                            !(rooms.cornerRooms.Contains(level[y, x].plan) &&
                              rooms.cornerRooms.Contains(level[y, x + 1].plan)))
                        {
                            map.SetTile(new Vector3Int(bound.xMax - 1, 0, 0), null);
                            map.SetTile(new Vector3Int(bound.xMax - 1, -1, 0), null);
                            Tilemap r_map = level[y, x + 1].room.transform.GetChild(1).gameObject
                                .GetComponent<Tilemap>();
                            BoundsInt r_bound = map.cellBounds;
                            r_map.SetTile(new Vector3Int(r_bound.xMin, 0, 0), null);
                            r_map.SetTile(new Vector3Int(r_bound.xMin, -1, 0), null);
                        }
                    }
                }
            }
        }
    }

    public void RemoveLevel()
    {
        for (int y = 0; y < levelSize; y++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                if (!level[y, x].cleared)
                {
                    foreach (var e in level[y, x].enemies)
                    {
                        if (e != null)
                        {
                            Destroy(e);
                        }
                    }
                }

                Destroy(level[y, x].room);
            }
        }
    }
}