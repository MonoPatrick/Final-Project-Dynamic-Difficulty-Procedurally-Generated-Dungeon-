using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGeneration dungeonGeneration;
    public RoomManager roomManager;
    // Start is called before the first frame update
    void Start()
    {
        dungeonGeneration.initialiseDungeon();
        dungeonGeneration.placeEntrance();
        dungeonGeneration.generateCriticalPath(dungeonGeneration.start, dungeonGeneration.critialPathLength, 0);
        dungeonGeneration.PrintDungeon();
        roomManager.overwriteNodes();
        dungeonGeneration.PrintDungeon();
        roomManager.PlacePathObjects();
    }


}
