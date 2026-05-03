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

    bool keyRoom = false;
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
        /*
        in this function the grammars set in dungeon generation are 
        turned into grammars that'll be used later on to instantiate the room type
        */
        for (int x = 0; x < dungeonGeneration.dimensions.x; x++)
        {
            for (int y = 0; y < dungeonGeneration.dimensions.y; y++)
            {
                string cell = dungeonGeneration.dungeon[x][y];
                if (cell != "0" && cell != "s" && cell != "B" && cell != "B0")
                {
                    if (cell == "f")
                    {
                        dungeonGeneration.dungeon[x][y] = "f";// final room
                    }
                    else
                    {
                        dungeonGeneration.dungeon[x][y] = "R"; // everything else is just an empty room and except 0, start, B for branch and b0 for start branch
                    }
                }
            }
        }

        for (int x = 0; x < dungeonGeneration.dimensions.x; x++)
        {
            for (int y = 0; y < dungeonGeneration.dimensions.y; y++)
            {
                //set room types to R transform the room
                if (dungeonGeneration.dungeon[x][y] == "R") // all rooms in the critical path are now R, it is capital as it'll be replaced in this if statement
                {
                    bool placed = false;

                    while (!placed)
                    {
                        int value = Random.Range(0, 3); // 0, 1, or 2

                        switch (value) // the rooms will be selected from enemy room, treasure room and p room
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
                    if (dungeonGeneration.dungeon[x][y] == "t") // if the number of enemy rooms is not met then replace treasure rooms with e
                    {
                        dungeonGeneration.dungeon[x][y] = "e";
                        numberOfEnemyRooms++;
                    }
                }

            }
        }

        Vector2Int endRoom = dungeonGeneration.criticalPathRooms[dungeonGeneration.criticalPathRooms.Count - 1];
        dungeonGeneration.dungeon[endRoom.x][endRoom.y] = "f"; // just to make sure the room at the end is f


    }
    GameObject GetRoomFromTemplates(Vector2Int entry, Vector2Int exit, Vector2Int extra, Vector2Int current )
    {
        

        if (extra != Vector2Int.zero) // if extra path isn't equal to anything then just move on
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
          

            return templates.TopBottomRooms[0]; // top and bottom straight room

        }

        if ((entry == Vector2Int.left && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.left))
        {
            return templates.LeftRightRooms[0];// left and right straight room
        }

        // CORNERS
        if ((entry == Vector2Int.up && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.up))
        {
            return templates.TopRightRooms[0]; // top and right corner room
        }

        if ((entry == Vector2Int.up && exit == Vector2Int.left) ||
            (entry == Vector2Int.left && exit == Vector2Int.up))
        {
            return templates.TopLeftRooms[0]; // top and left corner room
        }

        if ((entry == Vector2Int.down && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.down))
        {
            return templates.BottomRightRooms[0]; // bottom and right corner room
        }

        if ((entry == Vector2Int.down && exit == Vector2Int.left) ||
            (entry == Vector2Int.left && exit == Vector2Int.down))
        {
            return templates.BottomLeftRooms[0]; // bottom and left corner room
        }


        if (exit == Vector2Int.up)
            return templates.TopRooms[0]; // top dead end room

        if (exit == Vector2Int.down)
            return templates.BottomRooms[0]; // bottom dead end room

        if (exit == Vector2Int.left)
            return templates.LeftRooms[0]; // left dead end room

        if (exit == Vector2Int.right)
            return templates.RightRooms[0]; // right dead end room

        return null;
    }

    public void PlacePathObjects()
    {

        for (int i = 0; i < dungeonGeneration.criticalPathRooms.Count; i++) //this loops through the amount of critical path rooms
        {
            Vector2Int current = dungeonGeneration.criticalPathRooms[i]; // current is the position of the room in the critical path that we're currently looking at, this is used to determine which room to spawn and where to spawn it
            Vector2Int prev; //prev is the position of the previous room in the critical path of the current room
            // these are used to find the entrance and exit directions of the room 
           

            if (i > 0)
            {
                prev = dungeonGeneration.criticalPathRooms[i - 1]; //sets prev to the previous room
            }
            else
            {
                prev = current; // if i is 0 then there is no previous room which means previous room will be current meaning the exit will the same as the entrance making a dead end room 
            }
            Vector2Int next;
            if (i < dungeonGeneration.criticalPathRooms.Count - 1)
            {
                next = dungeonGeneration.criticalPathRooms[i + 1]; //the next room in the critical path is set to next getting the exit direction of the room
            }
            else
            {
                next = current; // if next is at the end then it'll be a dead end
            }


            Vector2Int entryDir = -(current - prev); // gives the direction from previous to current
            Vector2Int exitDir = (next - current); // exitDir = direction you leave the room toward.
            if (i == dungeonGeneration.criticalPathRooms.Count - 1)
            {
                exitDir = entryDir; // if it's the last room then the exit direction will be the same as the entry direction making it a dead end
            }
            else
            {
                exitDir = (next - current); //the exit direction is the direction from current to next
            }
            Vector3 position = new Vector3(current.x * 6, current.y * 6, 0); /*the position of the room is set to the current room's position multiplied by 6,
                                                                               this is because the rooms are 6 units apart from each other in the grid, this is used to determine where to spawn the
                                                                               room and the objects in it*/

            string cell = dungeonGeneration.dungeon[current.x][current.y]; //the cell is the type of room which is taken from the grammar set in overwrite nodes
            Vector2Int extraDir = Vector2Int.zero; // initialises the extra direction to zero so if extraDir in check3WayPath is 0 only do 2 way room

            extraDir = check3WayPath(entryDir,exitDir, current); //checks if path is going into branch so a 3 way path can be made

            setRooms(cell, entryDir, exitDir, extraDir, current); // initialises the ways of the room





        }
        foreach (List<Vector2Int> branch in dungeonGeneration.branchPaths)
        {
            for (int i = 1; i < branch.Count; i++)// this starts at 1 because the first room in the branch is the same as the critical path room so it doesn't need to be set again
            {
                Vector2Int current = branch[i];
                Vector2Int prev;


                if (i > 0)
                {
                    prev = branch[i - 1];// the previous room in the branch is set to prev getting the entrance direction of the room
                }
                else
                {
                    // find which neighbour is the main path
                    Vector2Int[] dirs = { // the 4 main directions
                        Vector2Int.up,
                        Vector2Int.down,
                        Vector2Int.left,
                        Vector2Int.right
                    };

                    prev = current; //

                    foreach (Vector2Int dir in dirs)// loop through the 4 main directions to find which one is the main path connection
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
                                if (neighbour == "BK")
                                {
                                    keyRoom = true;
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
    void setRooms(string cell, Vector2Int entryDir, Vector2Int exitDir, Vector2Int extraDir, Vector2Int current)
    {
        GameObject room = GetRoomFromTemplates(entryDir, exitDir, extraDir, current);
        Vector3 position = new Vector3(current.x * size, current.y * size, 0);

        if (room != null)
        {
            Instantiate(room, position, Quaternion.identity);
        }

        if (cell == "s")
        {
            playerpos.position = new Vector3(position.x, position.y, playerpos.position.z);
            Instantiate(templates.startTilePrefab, position, Quaternion.identity); // the start room is set to the position of the current room and the player is also set to this position, this is because the player starts in the start room
        }

        if (cell == "e")
        {
            Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);

            Vector3 center = new Vector3(current.x * size, current.y * size, 0);
            nodeGraph.CreateNodes(center); // run the create nodes function in node graph to spawn the nodes for the enemies to move around in, this is done here because the enemy rooms are the only rooms that need nodes
        }

        if (cell == "f")
        {
           
            Instantiate(templates.endTilePrefab, position, Quaternion.identity); // the end room is set to the position of the current room and the end tile prefab is also set to this position, this is because the player needs to reach the end room to win
            if (keyRoom)
            {
                Instantiate(templates.locked, position, Quaternion.identity); // if the key room has been placed then place the key room in the end room so the player needs to get the key to win
            }
            
            
        }

        if (cell == "p")
        {
            int random = UnityEngine.Random.Range(0, 3);

            if (random == 0 || random == 2)
            {
                Instantiate(templates.floor, position, Quaternion.identity);
            }
            if (random == 1)
            {
                Instantiate(templates.treasureRoom, position, Quaternion.identity);
            }

        }
        if (cell == "t")
        {
            
            
                Instantiate(templates.treasureRoom, position, Quaternion.identity);
            

        }

        if (cell == "B")
        {
            Vector3 center = new Vector3(current.x * size, current.y * size, 0);
            Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);
            nodeGraph.CreateNodes(center);
        }

        if (cell == "BK")
        {
            keyRoom = true;
            Instantiate(templates.keyRoom, position, Quaternion.identity);
           
        }

    }
    public void checkForKeyRoom()
            {
                for (int x = 0; x < dungeonGeneration.dimensions.x; x++)
                {
                    for (int y = 0; y < dungeonGeneration.dimensions.y; y++)
                    {
                        if (dungeonGeneration.dungeon[x][y] == "BK")
                        {
                            keyRoom = true;
                        }
                    }
                }
            }
    Vector2Int check3WayPath(Vector2Int entryDir, Vector2Int exitDir, Vector2Int current)
    {
        foreach (List<Vector2Int> branch in dungeonGeneration.branchPaths)
        {
            if (branch.Count < 2) continue;

            // Only the actual critical path room that owns the branch
            // is allowed to become a 3-way room.
            if (current != branch[0])
                continue;

            Vector2Int branchStart = branch[1]; // the first room in the branch is the same as the critical path room so we check the second room in the branch to find the direction of the branch

            Vector2Int diff = branchStart - current; // the direction of the branch is the difference between the current room and the first room in the branch

            // Must be directly adjacent
            if ((Mathf.Abs(diff.x) + Mathf.Abs(diff.y)) != 1) // if the difference in x and y is not 1 then it's not directly adjacent meaning it can't be a 3 way path
                continue;

            // Don't reuse the normal critical path entry/exit
            if (diff == entryDir || diff == exitDir)
                continue;

            return diff;
        }

        return Vector2Int.zero;
    }


}
