using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public int roomCount;
    public int levelSize;

    private GameObject[,] level;
    private Rooms rooms;
    
    // Start is called before the first frame update
    void Start()
    {
        level = new GameObject[levelSize,levelSize];
        rooms = (Rooms)GameObject.FindGameObjectWithTag("Rooms").GetComponent(typeof(Rooms));
        level[level.GetLength(0) / 2, level.GetLength(1) / 2] = rooms.startRoom;
        if (roomCount > levelSize * levelSize)
        {
            roomCount = levelSize * levelSize;
        }
        
        foreach (int i in Enumerable.Range(0, level.GetLength(0)-1).OrderBy(x => Random.Range(0,level.GetLength(0))))
        {
            Console.WriteLine(i);
        }
        
        for (int r = 1; r < roomCount; r++)
        {
            foreach (int i in Enumerable.Range(0, level.GetLength(0)-1).OrderBy(x => Random.Range(0,level.GetLength(0))))
            {
                foreach (int j in Enumerable.Range(0, level.GetLength(1)-1).OrderBy(x => Random.Range(0,level.GetLength(1))))
                {
                    if ((level[i, j] == null) &&
                        (i > 0 && level[i - 1, j] != null) ||                               //north
                        (j > 0 && level[i, j - 1] != null) ||                               //west
                        (i < level.GetLength(0) - 1 && level[i + 1, j] != null) ||  //south
                        (j < level.GetLength(1) - 1 && level[i, j + 1] != null))    //east
                    {
                        level[i, j] = rooms.normalRooms[Random.Range(0,rooms.normalRooms.Length)];
                        goto Next;
                    }
                }
            }
            Next: ;
        }
        String output = "";
        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {
                output += level[i, j] != null ? "R" : "N";
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
