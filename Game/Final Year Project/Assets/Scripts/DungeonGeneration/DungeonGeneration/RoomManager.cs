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
    public int size = 7;


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
                if (cell != "0" && cell != "s" && cell != "B" && cell != "B0")
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

        Vector2Int endRoom = dungeonGeneration.criticalPathRooms[dungeonGeneration.criticalPathRooms.Count - 1];
        dungeonGeneration.dungeon[endRoom.x][endRoom.y] = "f";


    }
    GameObject GetRoomFromTemplates(Vector2Int entry, Vector2Int exit, Vector2Int extra, Vector2Int current )
    {
        if (dungeonGeneration.dungeon[current.x][current.y] == "0")
        {
            return templates.barricade;
        }
            if (dungeonGeneration.dungeon[current.x][current.y] == "B0")
        {
            //return templates.test[0];
        }

        if (extra != Vector2Int.zero)
        {
            // 3 path rooms 
            if ((entry == Vector2Int.up || exit == Vector2Int.up || extra == Vector2Int.up) &&
                (entry == Vector2Int.left || exit == Vector2Int.left || extra == Vector2Int.left) &&
                (entry == Vector2Int.right || exit == Vector2Int.right || extra == Vector2Int.right)
    )
            {
                return templates.LeftTopRightRoom;
            }

            // DOWN + LEFT + RIGHT (missing UP)
            if ((entry == Vector2Int.down || exit == Vector2Int.down || extra == Vector2Int.down) &&
                (entry == Vector2Int.left || exit == Vector2Int.left || extra == Vector2Int.left) &&
                (entry == Vector2Int.right || exit == Vector2Int.right || extra == Vector2Int.right)
            )
            {
                return templates.LeftBottomRightRoom;
            }

            // LEFT + UP + DOWN (missing RIGHT)
            if ((entry == Vector2Int.left || exit == Vector2Int.left || extra == Vector2Int.left) &&
                (entry == Vector2Int.up || exit == Vector2Int.up || extra == Vector2Int.up) &&
                (entry == Vector2Int.down || exit == Vector2Int.down || extra == Vector2Int.down)
            )
            {
                return templates.LeftTopBottomRoom;
            }

            // RIGHT + UP + DOWN (missing LEFT)
            if ((entry == Vector2Int.right || exit == Vector2Int.right || extra == Vector2Int.right) &&
                (entry == Vector2Int.up || exit == Vector2Int.up || extra == Vector2Int.up) &&
                (entry == Vector2Int.down || exit == Vector2Int.down || extra == Vector2Int.down)
            )
            {
                return templates.RightTopBottomRoom;
            }
            // UP + DOWN + RIGHT (missing LEFT)
            if ((entry == Vector2Int.up || exit == Vector2Int.up || extra == Vector2Int.up) &&
                (entry == Vector2Int.down || exit == Vector2Int.down || extra == Vector2Int.down) &&
                (entry == Vector2Int.right || exit == Vector2Int.right || extra == Vector2Int.right))
            {
                return templates.RightTopBottomRoom;
            }
            // UP + DOWN + LEFT (missing RIGHT)
            if ((entry == Vector2Int.up || exit == Vector2Int.up || extra == Vector2Int.up) &&
                (entry == Vector2Int.down || exit == Vector2Int.down || extra == Vector2Int.down) &&
                (entry == Vector2Int.left || exit == Vector2Int.left || extra == Vector2Int.left))
            {
                return templates.LeftTopBottomRoom;
            }
        }
        
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
        for (int i = 0; i < dungeonGeneration.criticalPathRooms.Count; i++)
        {


            

            Vector2Int current = dungeonGeneration.criticalPathRooms[i];
            Vector2Int prev;
           

            if (i > 0)
            {
                prev = dungeonGeneration.criticalPathRooms[i - 1];
            }
            else
            {
                prev = current;
            }
            Vector2Int next;
            if (i < dungeonGeneration.criticalPathRooms.Count - 1)
            {
                next = dungeonGeneration.criticalPathRooms[i + 1];
            }
            else
            {
                next = current;
            }


            Vector2Int entryDir = -(current - prev); // gives the direction from previous to current
            Vector2Int exitDir = (next - current); // exitDir = direction you leave the room toward.
            if (i == dungeonGeneration.criticalPathRooms.Count - 1)
            {
                exitDir = entryDir;
            }
            else
            {
                exitDir = (next - current);
            }
            Vector3 position = new Vector3(current.x * 6, current.y * 6, 0);

            string cell = dungeonGeneration.dungeon[current.x][current.y];
            Vector2Int extraDir = Vector2Int.zero;

            extraDir = check3WayPath(entryDir,exitDir, current);

            setRooms(cell, entryDir, exitDir, extraDir, current);





        }
        foreach (List<Vector2Int> branch in dungeonGeneration.branchPaths)
        {
            for (int i = 0; i < branch.Count; i++)
            {
                Vector2Int current = branch[i];
                Vector2Int prev;


                if (i > 0)
                {
                    prev = branch[i - 1];
                }
                else
                {
                    // find which neighbour is the main path
                    Vector2Int[] dirs = {
                        Vector2Int.up,
                        Vector2Int.down,
                        Vector2Int.left,
                        Vector2Int.right
                    };

                    prev = current;

                    foreach (Vector2Int dir in dirs)
                    {
                        Vector2Int check = current + dir;

                        if (check.x >= 0 && check.x < dungeonGeneration.dimensions.x &&
                            check.y >= 0 && check.y < dungeonGeneration.dimensions.y)
                        {
                            string neighbour = dungeonGeneration.dungeon[check.x][check.y];
                            if (dungeonGeneration.dungeon[current.x][current.y] == "B0")
                            {
                                if (neighbour != "0" && neighbour != "B" && neighbour != "BK" && neighbour != "B0")
                                {
                                    // this is the main path connection
                                    prev = check;
                                    break;
                                }
                                // Prevent B0 rooms from connecting to each other
                                if (neighbour == "B0")
                                {
                                    continue;
                                }
                            }
                            if (neighbour != "0" && neighbour != "B" && neighbour != "BK")
                            {
                                // this is the main path connection
                                prev = check;
                                break;
                            }
                            if (neighbour != "0" && neighbour != "B" && neighbour != "BK")
                            {
                                // this is the main path connection
                                prev = check;
                                break;
                            }
                        }
                    }
                }

                Vector2Int next;
                if (i < branch.Count - 1)
                {
                    next = branch[i + 1];
                }
                else
                {
                    next = current;
                }


                Vector2Int entryDir = -(current - prev); // gives the direction from previous to current
                Vector2Int exitDir = (next - current); // exitDir = direction you leave the room toward.
                if (i == branch.Count - 1)
                {
                    exitDir = entryDir;
                }
                else
                {
                    exitDir = (next - current);
                }

                

                string cell = dungeonGeneration.dungeon[current.x][current.y];
                Vector2Int extraDir = Vector2Int.zero;

                setRooms(cell, entryDir, exitDir,extraDir, current);







            }
        }
    }
    void setRooms(string cell, Vector2Int entryDir, Vector2Int exitDir, Vector2Int extraDir, Vector2Int current )
    {
        GameObject room = GetRoomFromTemplates(entryDir, exitDir, extraDir, current);
        Vector3 position = new Vector3(current.x * size, current.y * size, 0);
        if (room != null)
        {
            Instantiate(room, position, Quaternion.identity);
        }
        if (cell == "0")
        {
            Instantiate(room, position, Quaternion.identity);
        }

        // Start & End override
        
        if (cell == "s")
        {
            playerpos.position = new Vector3(position.x, position.y, playerpos.position.z);
            Instantiate(templates.startTilePrefab, position, Quaternion.identity);
        }
        if (cell == "e")
        {
            int obstacles;

            Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);
            Vector3 center = new Vector3(current.x * size, current.y * size, 0); // gets the center
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

        
        if (room != null)
        {
            Instantiate(room, position, Quaternion.identity);
        }
        // Handle branch-specific rooms
        if (cell == "t" || cell == "p")
        {
            Instantiate(templates.floor, position, Quaternion.identity);
        }

        

        if (cell == "B")
        {
            Vector3 center = new Vector3(current.x * size, current.y * size, 0);
            Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);
            nodeGraph.CreateNodes(center);
        }
        if (cell == "BK")
        {
            Instantiate(templates.keyRoom, position, Quaternion.identity);

        }

    }
    Vector2Int check3WayPath(Vector2Int entryDir, Vector2Int exitDir, Vector2Int current)
    {
        foreach (var branch in dungeonGeneration.branchPaths)
        {
            if (branch.Count == 0) continue;

            Vector2Int branchStart = branch[0]; // get the start of the branch (the connection point to the main path)
            Vector2Int diff = branchStart - current; // get the direction from the current room to the start of the branch

            // Must be directly adjacent
            if ((Mathf.Abs(diff.x) + Mathf.Abs(diff.y)) != 1)
                continue;

            // Only connect to actual branch entrance
           //if (dungeonGeneration.dungeon[branchStart.x][branchStart.y] != "B0")
            //    continue;

            // Prevent fake 3-ways (don’t reuse entry/exit directions)
            if (diff == entryDir || diff == exitDir)
                continue;

            return diff;
        }

        return Vector2Int.zero;
    }

  
}
