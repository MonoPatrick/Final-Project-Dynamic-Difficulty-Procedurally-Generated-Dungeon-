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
        bool success = false;

        while (!success)
        {
            dungeonGeneration.initialiseDungeon();
            dungeonGeneration.placeEntrance();
            success = dungeonGeneration.generateCriticalPath(dungeonGeneration.start, dungeonGeneration.critialPathLength, 0);
            
        }
        success = false;

       
        //success = dungeonGeneration.GenerateBranch();
        



        dungeonGeneration.PrintDungeon();
        roomManager.overwriteNodes();
        dungeonGeneration.generateBranches();
        dungeonGeneration.PrintDungeon();
        roomManager.PlacePathObjects();

    }


}
