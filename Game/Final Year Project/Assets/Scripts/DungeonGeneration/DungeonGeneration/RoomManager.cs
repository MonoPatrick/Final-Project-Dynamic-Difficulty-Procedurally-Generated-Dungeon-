using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public DungeonGeneration dungeonGeneration;
    

    int enemyRooms = 6;
    int numberOfEnemyRooms = 0;

    int TreasureRooms = 20;
    int numberOfTreasureRooms = 0;

    public RoomTemplates templates;
    public Transform playerpos;

    public NodeGraph nodeGraph;

    bool bigRoom;
    bool openTop;
    bool openBottom;



    void Start()
    {
        enemyRooms = (dungeonGeneration.critialPathLength / 2) + (dungeonGeneration.critialPathLength / 4);
    }
    public void overwriteNodes()
    {

        for (int x = 0; x < dungeonGeneration.dimensions.x; x++)
        {
            for (int y = 0; y < dungeonGeneration.dimensions.y; y++)
            {
                string cell = dungeonGeneration.dungeon[x][y];
                if (cell != "0" && cell != "s")
                {
                    if (cell == "f")
                    {
                        dungeonGeneration.dungeon[x][y] = "f";
                    }
                    else
                    {
                        dungeonGeneration.dungeon[x][y] = "R";
                    }
                }
            }
        }

        for (int x = 0; x < dungeonGeneration.dimensions.x; x++)
        {
            for (int y = 0; y < dungeonGeneration.dimensions.y; y++)
            {
                //set room types to R transform the room
                if (dungeonGeneration.dungeon[x][y] == "R")
                {
                    bool placed = false;

                    while (!placed)
                    {
                        int value = Random.Range(0, 3); // 0, 1, or 2

                        switch (value)
                        {
                            case 0:
                                if (numberOfEnemyRooms < enemyRooms)
                                {
                                    dungeonGeneration.dungeon[x][y] = "e";
                                    numberOfEnemyRooms++;
                                    placed = true;
                                }
                                break;

                            case 1:
                                dungeonGeneration.dungeon[x][y] = "t";
                                placed = true;
                                break;

                            case 2:
                                dungeonGeneration.dungeon[x][y] = "p";
                                placed = true;
                                break;
                        }
                    }
                }

            }
        }
        for (int x = 0; x < dungeonGeneration.dimensions.x; x++)
        {
            for (int y = 0; y < dungeonGeneration.dimensions.y; y++)
            {
                if (numberOfEnemyRooms < enemyRooms)
                {
                    if (dungeonGeneration.dungeon[x][y] == "t")
                    {
                        dungeonGeneration.dungeon[x][y] = "e";
                        numberOfEnemyRooms++;
                    }
                }

            }
        }

        Vector2Int endRoom = dungeonGeneration.pathRooms[dungeonGeneration.pathRooms.Count - 1];
        dungeonGeneration.dungeon[endRoom.x][endRoom.y] = "f";


    }
    GameObject GetRoomFromTemplates(Vector2Int entry, Vector2Int exit)
    {
        // STRAIGHT ROOMS

        if ((entry == Vector2Int.up && exit == Vector2Int.down) ||
            (entry == Vector2Int.down && exit == Vector2Int.up))
        {
            if (openTop && !openBottom)
                return templates.TopBottomOpenTop;

            if (openBottom && !openTop)
                return templates.TopBottomOpenBottom;
            if (openTop && openBottom)
            {
                return templates.test[0];
            }

            return templates.TopBottomRooms[0];

            



        }

        if ((entry == Vector2Int.left && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.left))
        {
            return templates.LeftRightRooms[0];
        }

        // CORNERS
        if ((entry == Vector2Int.up && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.up))
        {
            return templates.TopRightRooms[0];
        }

        if ((entry == Vector2Int.up && exit == Vector2Int.left) ||
            (entry == Vector2Int.left && exit == Vector2Int.up))
        {
            return templates.TopLeftRooms[0];
        }

        if ((entry == Vector2Int.down && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.down))
        {
            return templates.BottomRightRooms[0];
        }

        if ((entry == Vector2Int.down && exit == Vector2Int.left) ||
            (entry == Vector2Int.left && exit == Vector2Int.down))
        {
            return templates.BottomLeftRooms[0];
        }


        if (exit == Vector2Int.up)
            return templates.TopRooms[0];

        if (exit == Vector2Int.down)
            return templates.BottomRooms[0];

        if (exit == Vector2Int.left)
            return templates.LeftRooms[0];

        if (exit == Vector2Int.right)
            return templates.RightRooms[0];

        return null;
    }

    public void PlacePathObjects()
    {
        for (int i = 0; i < dungeonGeneration.pathRooms.Count; i++)
        {


            

            Vector2Int current = dungeonGeneration.pathRooms[i];
            Vector2Int prev;
           

            if (i > 0)
            {
                prev = dungeonGeneration.pathRooms[i - 1];
            }
            else
            {
                prev = current;
            }
            Vector2Int next;
            if (i < dungeonGeneration.pathRooms.Count - 1)
            {
                next = dungeonGeneration.pathRooms[i + 1];
            }
            else
            {
                next = current;
            }


            Vector2Int entryDir = -(current - prev); // gives the direction from previous to current
            Vector2Int exitDir = (next - current); // exitDir = direction you leave the room toward.
            if (i == dungeonGeneration.pathRooms.Count - 1)
            {
                exitDir = entryDir;
            }
            else
            {
                exitDir = (next - current);
            }
            Vector3 position = new Vector3(current.x * 6, current.y * 6, 0);

            string cell = dungeonGeneration.dungeon[current.x][current.y];
            /*
            openTop = false;
            openBottom = false;
            string cell = dungeonGeneration.dungeon[current.x][current.y];

            if (i < dungeonGeneration.pathRooms.Count - 1)
            {
                Vector2Int nextRoom = dungeonGeneration.pathRooms[i + 1];
                string nextCell = dungeonGeneration.dungeon[nextRoom.x][nextRoom.y];

                if (cell == "e" && nextCell == "e")
                {
                    if (nextRoom.y > current.y) // next is ABOVE
                        openTop = true;

                    if (nextRoom.y < current.y) // next is BELOW
                        openBottom = true;
                }
            }
            if (i > 0)
            {
                Vector2Int prevRoom = dungeonGeneration.pathRooms[i - 1];
                string prevCell = dungeonGeneration.dungeon[prevRoom.x][prevRoom.y];

                if (cell == "e" && prevCell == "e")
                {
                    if (prevRoom.y > current.y) // prev is ABOVE
                        openTop = true;

                    if (prevRoom.y < current.y) // prev is BELOW
                        openBottom = true;
                }
            }
            */



            GameObject room = GetRoomFromTemplates(entryDir, exitDir);


            if (room != null)
            {
                Instantiate(room, position, Quaternion.identity);
            }

            // Start & End override
            if (cell == "t")
            {
                Instantiate(templates.floor, position, Quaternion.identity);

            }
            if (cell == "p")
            {
                Instantiate(templates.floor, position, Quaternion.identity);
            }
            if (cell == "s")
            {
                playerpos.position = new Vector3(position.x, position.y, playerpos.position.z);
                Instantiate(templates.startTilePrefab, position, Quaternion.identity);
            }
            if (cell == "e")
            {
                int obstacles;

                Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);
                Vector3 center = new Vector3(current.x * 6, current.y * 6, 0); // gets the center
                obstacles = UnityEngine.Random.Range(0, 5);
                for (int ii = 0; ii < obstacles; ii++)
                {
                    // Vector3 spawnPos = center + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0); // puts the obstacle in a random area in the room
                    //Instantiate(templates.objects[UnityEngine.Random.Range(0, templates.objects.Length)], spawnPos, Quaternion.identity);
                }
                //similar to the obstacles postioning set nodes in the room
                nodeGraph.CreateNodes(center);
            }

            if (cell == "f")
            {
                Instantiate(templates.endTilePrefab, position, Quaternion.identity);
            }
        }
    }
}
