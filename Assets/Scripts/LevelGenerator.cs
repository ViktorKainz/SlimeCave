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

    private GameObject[,] level;
    private Rooms rooms;

    // Start is called before the first frame update
    void Start()
    {
        level = new GameObject[levelSize, levelSize];
        rooms = (Rooms) GameObject.FindGameObjectWithTag("Rooms").GetComponent(typeof(Rooms));
        level[levelSize / 2, levelSize / 2] = rooms.startRoom;
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
                    if ((level[y, x] == null) && (
                        (y > 0 && level[y - 1, x] != null) || //north
                        (x > 0 && level[y, x - 1] != null) || //west
                        (y < level.GetLength(0) - 1 && level[y + 1, x] != null) || //south
                        (x < level.GetLength(1) - 1 && level[y, x + 1] != null))) //east
                    {
                        level[y, x] = rooms.normalRooms[Random.Range(0, rooms.normalRooms.Length)];
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
            if (level[y, x] == null || level[y, x] == rooms.startRoom) continue;
            if (y + 1 < levelSize && x + 1 < levelSize &&
                level[y + 1, x] == null &&
                level[y, x + 1] == null &&
                level[y + 1, x + 1] == null)
            {
                level[y, x] = rooms.cornerRooms[2];
                level[y + 1, x] = rooms.cornerRooms[0];
                level[y, x + 1] = rooms.cornerRooms[3];
                level[y + 1, x + 1] = rooms.cornerRooms[1];
            }
            else if (y + 1 < levelSize && x > 0 &&
                     level[y + 1, x] == null &&
                     level[y, x - 1] == null &&
                     level[y + 1, x - 1] == null)
            {
                level[y, x] = rooms.cornerRooms[3];
                level[y + 1, x] = rooms.cornerRooms[1];
                level[y, x - 1] = rooms.cornerRooms[2];
                level[y + 1, x - 1] = rooms.cornerRooms[0];
            }
            else if (y > 0 && x + 1 < levelSize && 
                     level[y - 1, x] == null &&
                     level[y, x + 1] == null &&
                     level[y - 1, x + 1] == null)
            {
                level[y, x] = rooms.cornerRooms[0];
                level[y - 1, x] = rooms.cornerRooms[2];
                level[y, x + 1] = rooms.cornerRooms[1];
                level[y - 1, x + 1] = rooms.cornerRooms[3];
            }
            else if (y > 0 && x > 0 &&
                     level[y - 1, x] == null &&
                     level[y, x - 1] == null &&
                     level[y - 1, x - 1] == null)
            {
                level[y, x] = rooms.cornerRooms[1];
                level[y - 1, x] = rooms.cornerRooms[3];
                level[y, x - 1] = rooms.cornerRooms[0];
                level[y - 1, x - 1] = rooms.cornerRooms[2];
            }
            else continue;
            boss = false;
        }

        for (int y = 0; y < levelSize; y++)
        {
            for (int x = 0; x < levelSize; x++)
            {
                if (level[y, x] != null)
                {
                    if (level[y, x] == rooms.startRoom)
                    {
                        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(x * width, y * height, -1);
                    }
                    level[y, x] = Instantiate(level[y, x], new Vector3(x * width, y * height), level[y, x].transform.rotation);
                    Tilemap map = level[y, x].transform.GetChild(1).gameObject.GetComponent<Tilemap>();
                    BoundsInt bound = map.cellBounds;
                    if (y > 0 && level[y - 1, x] != null)
                    {
                        map.SetTile(new Vector3Int(0, bound.yMin, 0), null);
                        map.SetTile(new Vector3Int(-1, bound.yMin, 0), null);
                    }

                    if (y < level.GetLength(0) - 1 && level[y + 1, x] != null)
                    {
                        map.SetTile(new Vector3Int(0, bound.yMax-1, 0), null);
                        map.SetTile(new Vector3Int(-1, bound.yMax-1, 0), null);
                    }

                    if (x > 0 && level[y, x - 1] != null)
                    {
                        map.SetTile(new Vector3Int(bound.xMin, 0, 0), null);
                        map.SetTile(new Vector3Int(bound.xMin, -1, 0), null);
                    }

                    if (x < level.GetLength(1) - 1 && level[y, x + 1] != null)
                    {
                        map.SetTile(new Vector3Int(bound.xMax-1, 0, 0), null);
                        map.SetTile(new Vector3Int(bound.xMax-1, -1, 0), null);
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