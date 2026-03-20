using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomNode
{
    Start, // starting room
    Boss, // boss room
    Treasure, // treasure room
    locked, //locked room
    key, // key for locked room
    Normal // room with enemies
}

public class DungeonRoom
{
    
    public Vector2Int gridPosition;
    public RoomNode roomNode;
    public List<DungeonRoom> connections = new List<DungeonRoom>();
    
   
}
