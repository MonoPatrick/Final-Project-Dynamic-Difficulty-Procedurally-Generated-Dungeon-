using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackUp : MonoBehaviour
{
    /*
     * 
     * 
     * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public Vector2Int dimensions;
    public Vector2Int start;

    public int critialPathLength;

    public List<List<string>> dungeon = new List<List<string>>(); // example with int cells
    public List<Vector2Int> criticalPathRooms = new List<Vector2Int>();
    public List<List<Vector2Int>> branchPaths = new List<List<Vector2Int>>();
    public List<List<Vector2Int>> roomDirections = new List<List<Vector2Int>>();
    public Vector2Int previousRoom;
    //difficulty
    public DynamicDifficultyAdjustment DDA;

    //branched paths
    int branches = 1;
    Vector2Int branchLength = new Vector2Int(2, 5);
    public List<Vector2Int> branchCandidates;

    public List<Vector2Int> keyRooms = new List<Vector2Int>();
    public List<Vector2Int> lockedRooms = new List<Vector2Int>();


    public void initialiseDungeon()
    {

        dungeon.Clear();
        for (int x = 0; x < dimensions.x + 1; x++)
        {
            dungeon.Add(new List<string>());
            for (int y = 0; y < dimensions.y + 1; y++)
            {
                dungeon[x].Add("0"); // initialize cell to 0 (or whatever you want)
            }
        }
        Debug.Log("Dungeon initialized with " + dimensions.x + "x" + dimensions.y);
        roomDirections.Clear();
        for (int x = 0; x < dimensions.x; x++)
        {
            roomDirections.Add(new List<Vector2Int>());

            for (int y = 0; y < dimensions.y + 1; y++)
            {
                roomDirections[x].Add(Vector2Int.zero);

            }


        }
        setCriticalPathLength();
    }

    public void setCriticalPathLength()
    {
        if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank1)
        {
            critialPathLength = 5;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank2)
        {
            critialPathLength = 10;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank3)
        {
            critialPathLength = 15;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank4)
        {
            critialPathLength = 20;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
        {
            critialPathLength = 25;
        }
    }

    public void placeEntrance()
    {
        if (start.x < 0 || start.x >= dimensions.x - 1)
        {
            start.x = UnityEngine.Random.Range(0, dimensions.x);
            return;
        }
        if (start.y < 0 || start.y >= dimensions.y - 1)
        {
            start.y = UnityEngine.Random.Range(0, dimensions.y);
            return;
        }
        dungeon[start.x][start.y] = "s";
        criticalPathRooms.Clear();
        criticalPathRooms.Add(start);
    }

    public bool generateCriticalPath(Vector2Int current, int length, int path)
    {
        if (length == 0)
        {
            return true; //
        }

        Vector2Int direction;

        int value = UnityEngine.Random.Range(0, 4);

        switch (value)
        {
            // sets the starting direction of the dungeon using the random value generated
            case 0:
                direction = Vector2Int.up; // up == (0,1)
                break;
            case 1:
                direction = Vector2Int.right; // right == (1,0)
                break;
            case 2:
                direction = Vector2Int.down; // down  == (0,-1)
                break;
            case 3:
                direction = Vector2Int.left; // left == (-1,0)
                break;
            default:
                direction = Vector2Int.up;
                break;
        }
        //Debug.Log(direction);
        //
        for (int i = 0; i < 4; i++)
        {
            Vector2Int next = current + direction;
            if (
                current.x + direction.x >= 0 && current.x + direction.x < dimensions.x - 1 &&
                current.y + direction.y >= 0 && current.y + direction.y < dimensions.y - 1 &&
                dungeon[current.x + direction.x][current.y + direction.y] == "0"
            )
            {
                
                
                dungeon[next.x][next.y] = length.ToString();
                branchCandidates.Add(next);
               
                //roomDirections[current.x][current.y] = direction;

                criticalPathRooms.Add(next);

                if (generateCriticalPath(next, length - 1, path + 1))
                {
                    return true;

                }
                else
                {
                    dungeon[current.x][current.y] = "0";
                    criticalPathRooms.RemoveAt(criticalPathRooms.Count - 1);
                    branchCandidates.Remove(current);
                    current -= direction;
                }
            }


            direction = new Vector2Int(-direction.y, direction.x); // counter-clockwise

        }

        return false;
    }

    public List<Vector2Int> GenerateBranch(Vector2Int start, int length)
    {
        List<Vector2Int> branch = new List<Vector2Int>();
        Vector2Int current = start;

        for (int i = 0; i < length; i++)
        {
            Vector2Int direction;

            int value = UnityEngine.Random.Range(0, 4);

            switch (value)
            {
                // sets the starting direction of the dungeon using the random value generated
                case 0:
                    direction = Vector2Int.up; // up == (0,1)
                    break;
                case 1:
                    direction = Vector2Int.right; // right == (1,0)
                    break;
                case 2:
                    direction = Vector2Int.down; // down  == (0,-1)
                    break;
                case 3:
                    direction = Vector2Int.left; // left == (-1,0)
                    break;
                default:
                    direction = Vector2Int.up;
                    break;
            }

            Vector2Int next = current + direction;

            if (
                current.x + direction.x >= 0 && current.x + direction.x < dimensions.x - 1 &&
                current.y + direction.y >= 0 && current.y + direction.y < dimensions.y - 1 &&
                dungeon[current.x + direction.x][current.y + direction.y] == "0"
            )
            {
                dungeon[next.x][next.y] = "B"; // B = branch
                branch.Add(next);

                current = next;
            }
            else
            {
                break;
            }
               

           
        }

        return branch;
    }

    public void PrintDungeon()
    {
        string dungeonAsString = "";
        string roomDirectionsAsString = "";

        for (int y = dimensions.y; y >= 0; y--) // print top row first
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                dungeonAsString += $"[{dungeon[x][y]}]";
            }
            dungeonAsString += "\n"; // newline after each row
        }
        for (int y = dimensions.y; y >= 0; y--) // print top row first
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                roomDirectionsAsString += $"[{roomDirections[x][y]}]";
            }
            dungeonAsString += "\n"; // newline after each row
        }


        Debug.Log(dungeonAsString);
        Debug.Log(roomDirectionsAsString);
    }
    public void generateBranches()
    {
        int branchCreated = 0;

        Vector2Int candidate;
        while(branchCreated < branches && 0 < branchCandidates.Count)
        {
            Vector2Int start = branchCandidates[Random.Range(0, branchCandidates.Count)];

            List<Vector2Int> branch = GenerateBranch(start, Random.Range(branchLength.x, branchLength.y));

            if (branch.Count > 0)
            {
                branchPaths.Add(branch);
                branchCreated++;
            }
            else
            {
                branchCandidates.Remove(start);
            }
        }
    }


}
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
                if (cell != "0" && cell != "s" && cell != "B")
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
    GameObject GetRoomFromTemplates(Vector2Int entry, Vector2Int exit, Vector2Int extra )
    {


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

            GameObject room = GetRoomFromTemplates(entryDir, exitDir,extraDir);


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
        foreach (var branch in dungeonGeneration.branchPaths)
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

                    foreach (var dir in dirs)
                    {
                        Vector2Int check = current + dir;

                        if (check.x >= 0 && check.x < dungeonGeneration.dimensions.x &&
                            check.y >= 0 && check.y < dungeonGeneration.dimensions.y)
                        {
                            string neighbour = dungeonGeneration.dungeon[check.x][check.y];

                            if (neighbour != "0" && neighbour != "B")
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

                Vector3 position = new Vector3(current.x * 6, current.y * 6, 0);

                string cell = dungeonGeneration.dungeon[current.x][current.y];
                Vector2Int extraDir = check3WayPath(entryDir, exitDir, current);

                GameObject room = GetRoomFromTemplates(entryDir, exitDir, extraDir);

                if (room != null)
                {
                    Instantiate(room, position, Quaternion.identity);
                }

                // Handle branch-specific rooms
                if (cell == "t" || cell == "p")
                {
                    Instantiate(templates.floor, position, Quaternion.identity);
                }

                if (cell == "e")
                {
                    Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);

                    Vector3 center = new Vector3(current.x * 6, current.y * 6, 0);
                    nodeGraph.CreateNodes(center);
                }

                if (cell == "B") 
                {
                    Instantiate(templates.enemyRooms[UnityEngine.Random.Range(0, templates.enemyRooms.Length)], position, Quaternion.identity);

                }


            }
        }
    }

    Vector2Int check3WayPath(Vector2Int entryDir, Vector2Int exitDir, Vector2Int current)
    {
        Vector2Int[] dirs = {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

        foreach (var dir in dirs)
        {
            Vector2Int check = current + dir;

            if (check.x >= 0 && check.x < dungeonGeneration.dimensions.x &&
                check.y >= 0 && check.y < dungeonGeneration.dimensions.y &&
                dungeonGeneration.dungeon[check.x][check.y] != "0")
            {
                // ignore entry/exit ONLY when assigning extra
                if (dir != entryDir && dir != exitDir)
                    return dir;
            }
        }

        return Vector2Int.zero;
    }
}

     */
}
