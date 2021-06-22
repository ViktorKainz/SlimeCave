using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rooms : MonoBehaviour
{
    public GameObject startRoom;
    public GameObject holeRoom;
    public GameObject[] normalRooms;
    public GameObject[] specialRooms;
    public GameObject[] cornerRooms;

    public GameObject[] enemies;
    public Tile[] decoration;
}
